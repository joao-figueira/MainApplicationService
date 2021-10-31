using System.Collections.Generic;

namespace MainApplicationService.Helpers
{
    public static class ErrorsDictionaryHelper
    {
        public static void AddToDictionary(IDictionary<string, List<string>> errorDictionary, string key, string error)
        {
            if (errorDictionary.ContainsKey(key))
            {
                errorDictionary[key].Add(error);
            } 
            else
            {
                errorDictionary[key] = new List<string>()
                {
                    error
                };
            }
        }
    }
}
