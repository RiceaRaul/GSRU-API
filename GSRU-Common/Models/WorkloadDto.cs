using GSRU_API.Common.Models;
using System.Dynamic;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace GSRU_Common.Models
{
    public class WorkloadDto
    {
        public int Id { get; set; } = -1;
        public string Employee { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
        public int Hour { get; set; }
        public int Total { get; set; }
        public int Day { get; set; }
        public float DayHours { get; set; }
    }

    public class Workload : DynamicObject
    {
        private readonly Dictionary<int, double> _properties = [];
        public int Id { get; set; } = -1;
        public string Employee { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
        public int Hour { get; set; }
        public int Total { get; set; }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (int.TryParse(binder.Name, out int key) && _properties.TryGetValue(key, out double value))
            {
                result = value;
                return true;
            }
            return base.TryGetMember(binder, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (int.TryParse(binder.Name, out int key) && value is double doubleValue && doubleValue >= 0 && doubleValue <= 1)
            {
                _properties[key] = doubleValue;
                return true;
            }
            return base.TrySetMember(binder, value);
        }

        public double this[int key]
        {
            get => _properties.TryGetValue(key, out double value) ? value : throw new KeyNotFoundException();
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 0 and 1.");
                }
                _properties[key] = value;
            }
        }

        public IDictionary<int, double> GetDynamicProperties() => _properties;
    }

    public class WorkLoadData : GenericError<string>
    {
        public int Id { get; set; }
        public int SprintId { get; set; }
        public int TotalHours { get; set; }
        public int TotalHoursSupport { get; set; }
        public float SupportPercent { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public List<Workload> Data { get; set; } = [];
    }

    public class WorkloadConverter : JsonConverter<Workload>
    {
        public override Workload Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var workload = new Workload();


            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return workload;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();
                    if (propertyName == ToCamelCase(nameof(Workload.Id)))
                    {
                        workload.Id = reader.GetInt32();
                    }
                    else if (propertyName == ToCamelCase(nameof(Workload.Employee)))
                    {
                        workload.Employee = reader.GetString();
                    }
                    else if (propertyName == ToCamelCase(nameof(Workload.EmployeeId)))
                    {
                        workload.EmployeeId = reader.GetInt32();
                    }
                    else if (propertyName == ToCamelCase(nameof(Workload.Hour)))
                    {
                        workload.Hour = reader.GetInt32();
                    }
                    else if (propertyName == ToCamelCase(nameof(Workload.Total)))
                    {
                        workload.Total = reader.GetInt32();
                    }
                    else if (int.TryParse(propertyName, out int key))
                    {
                        workload[key] = reader.GetDouble();
                    }
                }
            }
            

            return workload;
        }

        public override void Write(Utf8JsonWriter writer, Workload value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber(ToCamelCase(nameof(Workload.Id)), value.Id);
            writer.WriteString(ToCamelCase(nameof(Workload.Employee)), value.Employee);
            writer.WriteNumber(ToCamelCase(nameof(Workload.EmployeeId)), value.EmployeeId);
            writer.WriteNumber(ToCamelCase(nameof(Workload.Hour)), value.Hour);
            writer.WriteNumber(ToCamelCase(nameof(Workload.Total)), value.Total);

            foreach (var property in value.GetDynamicProperties())
            {
                writer.WriteNumber(property.Key.ToString(), property.Value);
            }

            writer.WriteEndObject();
        }

        //To camel case string
        public static string ToCamelCase(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return char.ToLowerInvariant(str[0]) + str[1..];
        }

    }
}
