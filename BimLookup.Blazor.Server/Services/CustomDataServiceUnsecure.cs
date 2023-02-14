namespace BimLookup.Blazor.Server.Services
{
    //TODO: This one isn't registred properly!?! Ask DevExpress
    //https://docs.devexpress.com/eXpressAppFramework/403850/backend-web-api-service/execute-custom-operations?p=net6
    using DevExpress.Data.Filtering;
    using DevExpress.ExpressApp;
    using DevExpress.ExpressApp.Core;
    using DevExpress.ExpressApp.DC;
    using DevExpress.ExpressApp.WebApi.Services;
    using Microsoft.AspNetCore.Authorization;
    using System;

    // ...

    public class CustomDataServiceUnsecure : IDataService
    {
        INonSecuredObjectSpaceFactory _objectSpaceFactory;
        private readonly DataService internalDataService;
        public CustomDataServiceUnsecure(INonSecuredObjectSpaceFactory objectSpaceFactory)
        {
            _objectSpaceFactory = objectSpaceFactory;
        }
        // POST
        public T CreateObject<T>(IObjectDelta<T> delta) where T : class
        {
            return internalDataService.CreateObject<T>(delta);
        }
        // POST with /key and associated property
        public void CreateRef<T>(string key, string navigationProperty, string relatedKey)
        {
            internalDataService.CreateRef<T>(key, navigationProperty, relatedKey);
        }
        // DELETE
        public T DeleteObject<T>(string key)
        {
            return internalDataService.DeleteObject<T>(key);
        }
        // DELETE with /key and associated property
        public void DeleteRef<TEntity>(string key, string navigationProperty, string relatedKey = null)
        {
            internalDataService.DeleteRef<TEntity>(key, navigationProperty, relatedKey);
        }
        // For internal use.
        public IEnumerable<T> GetObjects<T>(CriteriaOperator criteriaOperator)
        {
            return internalDataService.GetObjects<T>(criteriaOperator);
        }
        // GET
        [Authorize]
        public IQueryable<T> GetObjectsQuery<T>()
        {
            IObjectSpace newObjectSpace = _objectSpaceFactory.CreateNonSecuredObjectSpace<T>();
            var test = newObjectSpace.GetObjectsQuery<T>();
            return test;
        }
        // GET with /key
        public T GetObjectByKey<T>(string key)
        {
            return internalDataService.GetObjectByKey<T>(key);
        }
        // GET with /key and associated property
        public object GetRef<T>(string key, string navigationProperty)
        {
            return internalDataService.GetRef<T>(key, navigationProperty);
        }
        // PATCH
        public T PatchObject<T>(string key, IObjectDelta<T> delta) where T : class
        {
            return internalDataService.PatchObject<T>(key, delta);
        }
        // PUT
        public T UpdateObject<T>(string key, IObjectDelta<T> delta) where T : class
        {
            return internalDataService.UpdateObject<T>(key, delta);
        }

        public IObjectSpace GetObjectSpace(Type objectType)
        {
            return internalDataService.GetObjectSpace(objectType);
        }
    }
}
