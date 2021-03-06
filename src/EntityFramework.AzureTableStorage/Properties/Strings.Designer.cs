// <auto-generated />
namespace Microsoft.Data.Entity.AzureTableStorage
{
    using System.Globalization;
    using System.Reflection;
    using System.Resources;

    internal static class Strings
    {
        private static readonly ResourceManager _resourceManager
            = new ResourceManager("EntityFramework.AzureTableStorage.Strings", typeof(Strings).GetTypeInfo().Assembly);

        /// <summary>
        /// The string argument '{argumentName}' cannot be empty.
        /// </summary>
        internal static string ArgumentIsEmpty
        {
            get { return GetString("ArgumentIsEmpty"); }
        }

        /// <summary>
        /// The string argument '{argumentName}' cannot be empty.
        /// </summary>
        internal static string FormatArgumentIsEmpty(object argumentName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ArgumentIsEmpty", "argumentName"), argumentName);
        }

        /// <summary>
        /// The collection argument '{argumentName}' must contain at least one element.
        /// </summary>
        internal static string CollectionArgumentIsEmpty
        {
            get { return GetString("CollectionArgumentIsEmpty"); }
        }

        /// <summary>
        /// The collection argument '{argumentName}' must contain at least one element.
        /// </summary>
        internal static string FormatCollectionArgumentIsEmpty(object argumentName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("CollectionArgumentIsEmpty", "argumentName"), argumentName);
        }

        /// <summary>
        /// The value provided for argument '{argumentName}' must be a valid value of enum type '{enumType}'.
        /// </summary>
        internal static string InvalidEnumValue
        {
            get { return GetString("InvalidEnumValue"); }
        }

        /// <summary>
        /// The value provided for argument '{argumentName}' must be a valid value of enum type '{enumType}'.
        /// </summary>
        internal static string FormatInvalidEnumValue(object argumentName, object enumType)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("InvalidEnumValue", "argumentName", "enumType"), argumentName, enumType);
        }

        /// <summary>
        /// Cannot modify an Azure Storage account from within the Entity Framework
        /// </summary>
        internal static string CannotModifyAccount
        {
            get { return GetString("CannotModifyAccount"); }
        }

        /// <summary>
        /// Cannot modify an Azure Storage account from within the Entity Framework
        /// </summary>
        internal static string FormatCannotModifyAccount()
        {
            return GetString("CannotModifyAccount");
        }

        /// <summary>
        /// Cannot access a public setter and getter for the property '{propertyName}' of type '{typeName}'
        /// </summary>
        internal static string InvalidPoco
        {
            get { return GetString("InvalidPoco"); }
        }

        /// <summary>
        /// Cannot access a public setter and getter for the property '{propertyName}' of type '{typeName}'
        /// </summary>
        internal static string FormatInvalidPoco(object propertyName, object typeName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("InvalidPoco", "propertyName", "typeName"), propertyName, typeName);
        }

        /// <summary>
        /// This database cannot be used as an Azure Table Storage Database
        /// </summary>
        internal static string AtsDatabaseNotInUse
        {
            get { return GetString("AtsDatabaseNotInUse"); }
        }

        /// <summary>
        /// This database cannot be used as an Azure Table Storage Database
        /// </summary>
        internal static string FormatAtsDatabaseNotInUse()
        {
            return GetString("AtsDatabaseNotInUse");
        }

        /// <summary>
        /// Cannot read value of type '{typeName}' from '{accessName}'
        /// </summary>
        internal static string InvalidReadType
        {
            get { return GetString("InvalidReadType"); }
        }

        /// <summary>
        /// Cannot read value of type '{typeName}' from '{accessName}'
        /// </summary>
        internal static string FormatInvalidReadType(object typeName, object accessName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("InvalidReadType", "typeName", "accessName"), typeName, accessName);
        }

        /// <summary>
        /// A property with the storage name '{property}' on entity type '{entityType}' could not be found. Ensure that the property exists, has been included in the model, and has been configured with storage name specified.
        /// </summary>
        internal static string PropertyWithStorageNameNotFound
        {
            get { return GetString("PropertyWithStorageNameNotFound"); }
        }

        /// <summary>
        /// A property with the storage name '{property}' on entity type '{entityType}' could not be found. Ensure that the property exists, has been included in the model, and has been configured with storage name specified.
        /// </summary>
        internal static string FormatPropertyWithStorageNameNotFound(object property, object entityType)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("PropertyWithStorageNameNotFound", "property", "entityType"), property, entityType);
        }

        /// <summary>
        /// This connection cannot be used as an Azure Table Storage Connection
        /// </summary>
        internal static string AtsConnectionNotInUse
        {
            get { return GetString("AtsConnectionNotInUse"); }
        }

        /// <summary>
        /// This connection cannot be used as an Azure Table Storage Connection
        /// </summary>
        internal static string FormatAtsConnectionNotInUse()
        {
            return GetString("AtsConnectionNotInUse");
        }

        /// <summary>
        /// This query command has no filter string. This will pull the entire table into memory. Best practices recommend filtering with a partition and row key.
        /// </summary>
        internal static string MissingFilterString
        {
            get { return GetString("MissingFilterString"); }
        }

        /// <summary>
        /// This query command has no filter string. This will pull the entire table into memory. Best practices recommend filtering with a partition and row key.
        /// </summary>
        internal static string FormatMissingFilterString()
        {
            return GetString("MissingFilterString");
        }

        /// <summary>
        /// This query command has no partition key and/or row key in the query filter. This can produce a large table scan that will reduce performance and may increase costs. Best practices recommend filtering with a partition and row key.
        /// </summary>
        internal static string MissingPartitionOrRowKey
        {
            get { return GetString("MissingPartitionOrRowKey"); }
        }

        /// <summary>
        /// This query command has no partition key and/or row key in the query filter. This can produce a large table scan that will reduce performance and may increase costs. Best practices recommend filtering with a partition and row key.
        /// </summary>
        internal static string FormatMissingPartitionOrRowKey()
        {
            return GetString("MissingPartitionOrRowKey");
        }

        /// <summary>
        /// This entity has been modified on the server. Overwriting will destory changes that exist only on the server.
        /// </summary>
        internal static string ETagPreconditionFailed
        {
            get { return GetString("ETagPreconditionFailed"); }
        }

        /// <summary>
        /// This entity has been modified on the server. Overwriting will destory changes that exist only on the server.
        /// </summary>
        internal static string FormatETagPreconditionFailed()
        {
            return GetString("ETagPreconditionFailed");
        }

        /// <summary>
        /// Could not save changes. See inner exception for details.
        /// </summary>
        internal static string SaveChangesFailed
        {
            get { return GetString("SaveChangesFailed"); }
        }

        /// <summary>
        /// Could not save changes. See inner exception for details.
        /// </summary>
        internal static string FormatSaveChangesFailed()
        {
            return GetString("SaveChangesFailed");
        }

        /// <summary>
        /// Table or row not found on server
        /// </summary>
        internal static string ResourceNotFound
        {
            get { return GetString("ResourceNotFound"); }
        }

        /// <summary>
        /// Table or row not found on server
        /// </summary>
        internal static string FormatResourceNotFound()
        {
            return GetString("ResourceNotFound");
        }

        /// <summary>
        /// Table not found on server
        /// </summary>
        internal static string TableNotFound
        {
            get { return GetString("TableNotFound"); }
        }

        /// <summary>
        /// Table not found on server
        /// </summary>
        internal static string FormatTableNotFound()
        {
            return GetString("TableNotFound");
        }

        private static string GetString(string name, params string[] formatterNames)
        {
            var value = _resourceManager.GetString(name);

            System.Diagnostics.Debug.Assert(value != null);

            if (formatterNames != null)
            {
                for (var i = 0; i < formatterNames.Length; i++)
                {
                    value = value.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
                }
            }

            return value;
        }
    }
}
