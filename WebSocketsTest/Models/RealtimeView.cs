using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsTest.Models
{
    public class RealtimeView
    {
        private string _viewName = null;
        public string ViewName {
            get { return _viewName; }
        }
        private Type _modelType = null;
        public Type ModelType {
            get { return _modelType; }
        }

        public RealtimeView(string viewName, Type modelType) {
            if (viewName == null) throw new ArgumentNullException("viewName");
            if (modelType == null) throw new ArgumentNullException("modelType");
            if (viewName == string.Empty) throw new ArgumentException("ViewName cannot be empty", "viewName");
            if (!modelType.GetInterfaces().Contains(typeof(IRealtimeModel))) throw new ArgumentException("modelType is not an implementation of IRealtimeModel");

            _viewName = viewName;
            _modelType = modelType;
        }

        public class FilterContext
        {
            public bool IsWithinFilter(NameValueCollection filters)
            {
                throw new NotImplementedException();
            }
        }


        public FilterContext GetFilterContext(Type type, object instance) {
            throw new NotImplementedException();
        }


    }
}
