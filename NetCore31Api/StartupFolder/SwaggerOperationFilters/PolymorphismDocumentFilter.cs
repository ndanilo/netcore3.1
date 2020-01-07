//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//using Swashbuckle.AspNetCore.Swagger;
//using Swashbuckle.AspNetCore.SwaggerGen;

//namespace NetCore31Api.StartupFolder.SwaggerOperationFilters
//{
//    public class PolymorphismDocumentFilter<T> : IDocumentFilter
//    {
//        private PropertyInfo _discriminatorProperty;
//        public PolymorphismDocumentFilter(Expression<Func<T,object>> discriminatorExpression)
//        {
//            UnaryExpression expr = discriminatorExpression.Body as UnaryExpression;
//            MemberExpression member = expr.Operand as MemberExpression;
//            _discriminatorProperty = member.Member as PropertyInfo;
//        }
        
//        private void RegisterSubClasses(ISchemaRegistry schemaRegistry, Type abstractType)
//        {
//            string discriminatorName = _discriminatorProperty.Name;

//            var parentSchema = schemaRegistry.Definitions[abstractType.Name];

//            //set up a discriminator property (it must be required)
//            parentSchema.Discriminator = discriminatorName;
//            parentSchema.Required = new List<string> { discriminatorName };

//            //register all subclasses
//            var derivedTypes = abstractType.Assembly
//                                           .GetTypes()
//                                           .Where(x => abstractType != x && abstractType.IsAssignableFrom(x));

//            foreach (var item in derivedTypes)
//                schemaRegistry.GetOrRegister(item);
//        }

//        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
//        {
//            RegisterSubClasses(context.SchemaRegistry, typeof(T));
//        }
//    }
//}