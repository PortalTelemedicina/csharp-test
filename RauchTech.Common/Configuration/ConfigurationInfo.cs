using Microsoft.Extensions.Configuration;
using System;

namespace Demo.Common.Configuration
{
    public static class ConfigurationInfo
    {
        //Allows to get single or array values
        public static string[] GetValue(this IConfiguration config, string path)
        {
            IConfigurationSection section;

            string[] values = null;

            try
            {
                section = config.GetSection(path);

                values = (section.Value == null) ?
                            section.Get<string[]>() :
                            new string[] { section.Value };

                if (values.Length == 0)
                {
                    throw null;
                }
            }
            catch
            {
                throw new Exception($"Some problem occurred while trying to get the '{path}' Configuration");
            }

            return values;
        }
    }
}
