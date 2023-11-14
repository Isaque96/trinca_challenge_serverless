namespace Domain.Utils;

public abstract class Util
{
    public static T UpdateLogic<T>(T newEntity, T oldEntity, bool setAll = false)
    {
        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            if (!FullTypeTest(property.PropertyType))
                continue;
            
            var newValue = property.GetValue(newEntity);
            var oldValue = property.GetValue(oldEntity);

            if (!setAll && (newValue == null || newValue.Equals(oldValue)))
                continue;

            if (property.PropertyType == typeof(string) &&
                string.IsNullOrEmpty(newValue!.ToString()))
            {
                property.SetValue(oldEntity, oldValue);   
            }
            else if (property.PropertyType == typeof(DateTime) &&
                     ((DateTime)newValue! == DateTime.MinValue ||
                      (DateTime)newValue! == DateTime.MaxValue))
            {
                property.SetValue(oldEntity, oldValue);
            }
            else
            {
                property.SetValue(oldEntity, newValue);
            }
        }

        return oldEntity;
    }
    
    private static bool FullTypeTest(Type type)
    {
        return type.IsPrimitive ||
               type.IsEnum ||
               type == typeof(string) ||
               type == typeof(decimal) ||
               type == typeof(DateTime) ||
               type == typeof(DateTimeOffset) ||
               type == typeof(TimeSpan) ||
               type == typeof(Guid) ||
               IsNullableSimpleType(type);

        bool IsNullableSimpleType(Type t)
        {
            var underlyingType = Nullable.GetUnderlyingType(t);
            return underlyingType != null && FullTypeTest(underlyingType);
        }
    }
}