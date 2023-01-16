using System.Text.RegularExpressions;

namespace BankAPI.Models.GovernmentId;

public abstract class GovernmentId
{
    private GovernmentId(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public abstract string Name { get; }

    public abstract bool IsValid();

    public static GovernmentId Create(int typeId, string name, string value)
    {
        var matchedType = GovernmentIdType.Types.FirstOrDefault(t => t.Id == typeId);
        if (matchedType is null) return new InvalidGovernmentId(name, value);
        if (matchedType.Name.Equals(UnknownGovernmentId.TypeName, StringComparison.InvariantCultureIgnoreCase))
            return new UnknownGovernmentId(name, value);
        if (!matchedType.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            return new InvalidGovernmentId(name, value);

        return matchedType.Name switch
        {
            DriverLicense.TypeName => new DriverLicense(value),
            Passport.TypeName => new Passport(value),
            IdNumber.TypeName => new IdNumber(value),
            SocialSecurityNumber.TypeName => new SocialSecurityNumber(value),
            var _ => throw new ArgumentOutOfRangeException(nameof(typeId))
        };
    }

    public static GovernmentId FromRequest(GovernmentIdRequest request)
    {
        return Create(request.TypeId, request.Name, request.Value);
    }

    public GovernmentIdResponse ToResponse()
    {
        return new()
        {
            TypeId = GovernmentIdType.GetTypeId(Name),
            Name = Name,
            Value = Value
        };
    }

    public int GetTypeId()
    {
        return GovernmentIdType.GetTypeId(Name);
    }

    public static IReadOnlyList<GovernmentIdTypeResponse> GetAllIdTypesAsResponses()
    {
        return GovernmentIdType.Types.
            Select(t => new GovernmentIdTypeResponse
            {
                TypeId = t.Id,
                Name = t.Name
            }).
            ToList();
    }

    private sealed class InvalidGovernmentId : GovernmentId
    {
        public InvalidGovernmentId(string name, string value) : base(value)
        {
            Name = name;
        }

        public override string Name { get; }

        public override bool IsValid()
        {
            return false;
        }
    }

    private sealed class UnknownGovernmentId : GovernmentId
    {
        public const string TypeName = "Other";

        public UnknownGovernmentId(string name, string value) : base(value)
        {
            Name = name;
        }

        public override string Name { get; }

        public override bool IsValid()
        {
            return true;
        }
    }

    private sealed class DriverLicense : GovernmentId
    {
        public const string TypeName = "Driver License";

        public DriverLicense(string value) : base(value)
        {
        }

        public override string Name => TypeName;

        public override bool IsValid()
        {
            return Regex.IsMatch(Value, @"^[A-Za-z0-9]{4,9}$")  // length and alphanumeric
                   && Regex.IsMatch(Value, "^..[0-9]{2}")       // 3rd+4th are numeric
                   && Regex.IsMatch(Value, "(.*[0-9]){4}")      // at least 4 numeric
                   && !Regex.IsMatch(Value, "(.*[A-Za-z]){3}"); // no more than 2 alpha
        }
    }

    private sealed class Passport : GovernmentId
    {
        public const string TypeName = "Passport";

        public Passport(string value) : base(value)
        {
        }

        public override string Name => TypeName;

        public override bool IsValid()
        {
            return Regex.IsMatch(Value, @"^[a-zA-Z]{2}\d{7}$");
        }
    }

    private sealed class IdNumber : GovernmentId
    {
        public const string TypeName = "Government ID";

        public IdNumber(string value) : base(value)
        {
        }

        public override string Name => TypeName;

        public override bool IsValid()
        {
            return Regex.IsMatch(Value, @"^[a-zA-Z]{3}\d{6}$");
        }
    }

    private sealed class SocialSecurityNumber : GovernmentId
    {
        public const string TypeName = "Social Security Number";

        public SocialSecurityNumber(string value) : base(value)
        {
        }

        public override string Name => TypeName;

        public override bool IsValid()
        {
            return Regex.IsMatch(Value, @"^\d{11}$");
        }
    }

    public class GovernmentIdType
    {
        public static readonly IReadOnlyList<GovernmentIdType> Types = new[]
            {
                DriverLicense.TypeName,
                Passport.TypeName,
                IdNumber.TypeName,
                SocialSecurityNumber.TypeName,
                UnknownGovernmentId.TypeName
            }.
            Select((name, i) => new GovernmentIdType(i + 1, name)).
            ToList();

        private GovernmentIdType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }

        public string Name { get; }

        public static int GetTypeId(string name)
        {
            return (Types.FirstOrDefault(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    ?? Types.First(t => t.Name == UnknownGovernmentId.TypeName)).Id;
        }
    }
}