using OpenTelemetry.Trace;
using Rougamo.APM;
using Rougamo.Context;
using System.Diagnostics;

namespace Rougamo.OpenTelemetry
{
    /// <summary>
    /// OpenTelemetry activity
    /// </summary>
    public class OtelAttribute : MoAttribute
    {
        private Activity _activity;
        private string _parameters;

        /// <summary>
        /// use method full name if not set this property
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// record parameter and return value and working with <see cref="ApmIgnoreAttribute"/> if true, otherwise dot not record those and working with <see cref="ApmRecordAttribute"/>, default true
        /// </summary>
        public virtual bool RecordArguments { get; set; } = true;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void OnEntry(MethodContext context)
        {
            if (OtelSingleton.Options == null) return;

            var name = !string.IsNullOrEmpty(Name) ? Name :
                            (OtelSingleton.Options.ShortName ?
                                    $"{context.TargetType.Name}.{context.Method.Name}" :
                                    $"{context.TargetType.FullName}.{context.Method.Name}");
            _parameters = context.GetMethodParameters(OtelSingleton.Serializer, RecordArguments);
            _activity = OtelSingleton.Source.StartActivity(name);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void OnSuccess(MethodContext context)
        {
            if (_activity == null) return;

            var returnString = context.GetMethodReturnValue(OtelSingleton.Serializer, RecordArguments);
            RecordingArguments(_parameters, returnString);
            SetSuccessStatus();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void OnException(MethodContext context)
        {
            if (_activity == null) return;

            _activity.SetStatus(Status.Error);
            if (context.IsMuteExceptionForApm(RecordArguments))
            {
                if (!context.Exception.Data.Contains(OtelConstants.EXCEPTION_MARK))
                {
                    _activity.RecordException(context.Exception);
                    context.Exception.Data.Add(OtelConstants.EXCEPTION_MARK, null);
                }
            }
            else
            {
                _activity.RecordException(context.Exception);
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void OnExit(MethodContext context)
        {
            if (_activity == null) return;

            _activity.Dispose();
            _activity = null;
        }
        
        private void RecordingArguments(string parameters, string @return)
        {
            if (OtelSingleton.Options == null || _activity == null) return;

            switch (OtelSingleton.Options.ArgumentsStoreType)
            {
                case ArgumentsStoreType.Tag:
                    _activity.AddTag(OtelSingleton.Options.KeyNames.TagParameter, parameters);
                    _activity.AddTag(OtelSingleton.Options.KeyNames.TagReturn, @return);
                    break;
                case ArgumentsStoreType.Event:
                    _activity.AddEvent(new ActivityEvent(OtelSingleton.Options.KeyNames.EventArguments, default, new ActivityTagsCollection
                    {
                        { OtelSingleton.Options.KeyNames.TagParameter, parameters },
                        { OtelSingleton.Options.KeyNames.TagReturn, @return }
                    }));
                    break;
            }
        }

        private void SetSuccessStatus()
        {
            if (OtelSingleton.Options == null || _activity == null) return;

            var status = OtelSingleton.Options.SetOkStatusWhenSuccess ? Status.Ok : Status.Unset;
            _activity.SetStatus(status);
        }
    }
}
