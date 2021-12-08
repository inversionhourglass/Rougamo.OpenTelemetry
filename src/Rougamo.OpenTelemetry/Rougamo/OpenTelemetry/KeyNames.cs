namespace Rougamo.OpenTelemetry
{
    /// <summary>
    /// tags / events etc.. name
    /// </summary>
    public struct KeyNames
    {
        private const string TAG_KEY_PARAMETER = "parameters";
        private const string TAG_KEY_RETURN = "return";
        private const string EVENT_NAME_ARGUMENTS = "arguments";

        private string _tagParameter;
        private string _tagReturn;
        private string _eventArguments;

        /// <summary>
        /// parameter tag name
        /// </summary>
        public string TagParameter
        {
            get => _tagParameter ?? TAG_KEY_PARAMETER;
            set => _tagParameter = value;
        }

        /// <summary>
        /// return value tag name
        /// </summary>
        public string TagReturn
        {
            get => _tagReturn ?? TAG_KEY_RETURN;
            set => _tagReturn = value;
        }

        /// <summary>
        /// arguments(parameter and return value) event name
        /// </summary>
        public string EventArguments
        {
            get => _eventArguments ?? EVENT_NAME_ARGUMENTS;
            set => _eventArguments = value;
        }
    }
}
