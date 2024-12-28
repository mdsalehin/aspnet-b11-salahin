using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace XmlFormattingAssignment
{
    public static class XmlFormatter
    {
        public static string Convert(object obj)
        {
            if (obj == null)
                return string.Empty;

            StringBuilder xmlBuilder = new StringBuilder();
            Type objType = obj.GetType();
            string rootName = objType.Name;

            xmlBuilder.AppendLine($"<{rootName}>");
            SerializeObject(obj, xmlBuilder);
            xmlBuilder.AppendLine($"</{rootName}>");

            return xmlBuilder.ToString();
        }

        private static void SerializeObject(object obj, StringBuilder xmlBuilder)
        {
            if (obj == null)
                return;

            Type objType = obj.GetType();

            foreach (var property in objType.GetProperties())
            {
                string propertyName = property.Name;

                try
                {
                   


                    object? propertyValue = null;
                    if (property != null && obj != null)
                    {
                        if (property.GetIndexParameters().Length == 0)
                        {
                          
                            propertyValue = property.GetValue(obj);
                        }
                        else
                        {
    
                            int index = 0; 

                            propertyValue = property.GetValue(obj, new object[] { index });
                        }
                    }




                    if (propertyValue == null)
                    {
                        xmlBuilder.AppendLine($"<{propertyName}></{propertyName}>");
                    }
                    else if (propertyValue is string || propertyValue.GetType().IsPrimitive || propertyValue is DateTime)
                    {
                       
                        xmlBuilder.AppendLine($"<{propertyName}>{propertyValue}</{propertyName}>");
                    }
                    else if (propertyValue is IEnumerable collection && !(propertyValue is string))
                    {
                        
                        xmlBuilder.AppendLine($"<{propertyName}>");
                        foreach (var item in collection)
                        {
                            if (item is string)
                            {
                                xmlBuilder.AppendLine($"<{item.GetType().Name}>{item}</{item.GetType().Name}>");
                            }
                            else
                            {
                                string itemTypeName = item.GetType().Name;
                                xmlBuilder.AppendLine($"<{itemTypeName}>");
                                SerializeObject(item, xmlBuilder);
                                xmlBuilder.AppendLine($"</{itemTypeName}>");
                            }
                        }
                        xmlBuilder.AppendLine($"</{propertyName}>");
                    }
                    else
                    {
                      
                        xmlBuilder.AppendLine($"<{propertyName}>");
                        SerializeObject(propertyValue, xmlBuilder);
                        xmlBuilder.AppendLine($"</{propertyName}>");
                    }
                }
                catch
                {
                    
                    xmlBuilder.AppendLine($"<{propertyName}>Error retrieving value</{propertyName}>");
                }
            }
        }
    }
}
