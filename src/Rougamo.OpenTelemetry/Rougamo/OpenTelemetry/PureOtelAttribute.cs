﻿namespace Rougamo.OpenTelemetry
{
    /// <summary>
    /// inherited from <see cref="OtelAttribute"/>, change <see cref="RecordArguments"/> default value to false
    /// </summary>
    public class PureOtelAttribute : OtelAttribute
    {
        /// <inheritdoc/>
        public override bool RecordArguments { get; set; } = false;
    }
}
