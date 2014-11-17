using System.Collections.Generic;

namespace NotifierService.Processors
{
    public interface IProcessor
    {
        bool Process(object value);
        void Initialize(Dictionary<string, string> parameters);
    }
}
