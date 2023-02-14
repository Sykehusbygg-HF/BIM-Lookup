using System;
using DevExpress.ExpressApp.Xpo;

namespace BimLookup.Blazor.Server.Services {
    public class XpoDataStoreProviderAccessor {
        public IXpoDataStoreProvider DataStoreProvider { get; set; }
    }
}
