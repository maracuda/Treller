using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SKBKontur.Treller.WebApplication.Implementation.React
{
    public class ReactComponent
    {
        private static ReactComponent instance;

        public object Props { get; set; }
        public string ComponentName { get; set; }

        private ReactComponent(string componentName, object props)
        {
            ComponentName = componentName;
            Props = props;
        }

        public static ReactComponent Instance(string componentName, object props)
        {
            if (instance == null)
            {
                instance = new ReactComponent(componentName, props);
            }

            return instance;
        }

        public static ReactComponent Instance()
        {
            return instance;
        }

        public string GetId()
        {
            return $"react-{ComponentName}";
        }

        public string RenderJavaScript()
        {
            return string.Format("ReactDOM.render({0}, document.getElementById('root'));", GetComponentInitialiser());
        }

        private string GetComponentInitialiser()
        {
            var serializedObject = JsonConvert.SerializeObject(Props, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            });

            return $"React.createElement({ComponentName}, {serializedObject})";
        }
    }
}