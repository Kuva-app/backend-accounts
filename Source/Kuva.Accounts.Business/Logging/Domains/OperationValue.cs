using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Serilog.Events;

#nullable enable
namespace Kuva.Accounts.Business.Logging.Domains
{
    public class OperationValue : LogEventPropertyValue
    {
        private const string TypeName = "operation";

        public DateTimeOffset Timestamp { get; }
        public string Id { get; }
        public string Name { get; }
        public object Data { get; }
        
        public OperationValue(string name, object data)
        {
            Timestamp = DateTimeOffset.Now;
            Id = Guid.NewGuid().ToString("N");
            Name = name;
            Data = data;
        }

        public override void Render(TextWriter output, string? format = null, IFormatProvider? formatProvider = null)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));

            output.Write(TypeName);
            output.Write(" ");

            output.Write("{ ");

            output.Render(nameof(Timestamp), Timestamp, ", ");
            output.Render(nameof(Id), Id, ", ");
            output.Render(nameof(Name), Name, ", ");
            
            var typeOfData = Data.GetType();

            switch (Data)
            {
                case string dataStringValue:
                    output.Render(typeOfData.Name, dataStringValue);
                    break;
                case bool dataBooleanValue: 
                    output.Render(typeOfData.Name, dataBooleanValue);
                    break;
                case IFormattable dataForgettable:
                    output.Render(typeOfData.Name, dataForgettable.ToString(format, formatProvider ?? CultureInfo.InvariantCulture));
                    break;
                default:
                {
                    if (!typeOfData.IsValueType)
                    {
                        var properties = typeOfData.GetProperties();
                        output.Write(typeOfData.Name);
                        output.Write(": ");
                        output.Write("{ ");
                        var length = properties.Length - 1;
                        for (var i = 0; i < length; i++)
                        {
                            output.Render(properties[i].Name, properties[i].GetValue(Data) ?? "null", ", ");
                        }
                        var lastProperty = properties.Last();
                        output.Render(lastProperty.Name, lastProperty.GetValue(Data) ?? "null");
                        output.Write(" }");
                    }
                    break;
                }
            }
            output.Write(" }");
        }
    }
}
