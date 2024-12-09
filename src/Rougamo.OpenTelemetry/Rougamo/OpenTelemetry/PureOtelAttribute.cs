using Rougamo.Metadatas;

namespace Rougamo.OpenTelemetry
{
    /// <summary>
    /// inherited from <see cref="OtelAttribute"/>, change <see cref="RecordArguments"/> default value to false
    /// </summary>
    [Advice(Feature.Observe)]
    [Lifetime(Lifetime.Pooled)]
    public class PureOtelAttribute : OtelAttribute
    {
        /// <inheritdoc/>
        public override bool RecordArguments { get; set; } = false;

        /// <inheritdoc/>
        public override bool TryReset()
        {
            base.TryReset();
            RecordArguments = false;

            return true;
        }
    }
}
