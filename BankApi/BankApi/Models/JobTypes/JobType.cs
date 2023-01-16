namespace BankAPI.Models.JobTypes;

public sealed class JobType
{
    private const int UnknownJobTypeId = 1;
    private static int _nextId = UnknownJobTypeId + 1;

    private static readonly JobType UnknownJobType = new(UnknownJobTypeId, "Unknown");

    private JobType(int id, string name)
    {
        Id = id;
        Name = name;
    }

    private JobType(string name)
    {
        Id = _nextId++;
        Name = name;
    }

    public int Id { get; }

    public string Name { get; }

    private static IEnumerable<JobType> KnownJobTypes { get; } = new JobType[]
    {
        new("Aircraft dispatcher"),
        new("Aircraft mechanic"),
        new("Airline pilot"),
        new("Federal Air Marshal"),
        new("Flight attendant"),
        new("Transportation security screener"),
        new("Actor"),
        new("Artist"),
        new("Composer"),
        new("Museum archivist"),
        new("Musician"),
        new("Singer"),
        new("Accountant"),
        new("Administrative assistant"),
        new("Human resources specialist"),
        new("Manager"),
        new("Market research analyst"),
        new("College professor"),
        new("Instructional coordinator"),
        new("Librarian"),
        new("Teacher"),
        new("Substitute teacher"),
        new("Tutor"),
        new("Criminal Investigator"),
        new("Fish and game warden"),
        new("Police officer"),
        new("Security guard"),
        new("Media editor"),
        new("Public relations specialist"),
        new("Sound engineering technician"),
        new("Graphic designer"),
        new("Writer"),
        new("Doctor"),
        new("Nurse"),
        new("Paramedic"),
        new("Physician assistant"),
        new("Social worker"),
        new("Veterinarian"),
        new("Bank teller"),
        new("Hairdresser"),
        new("Personal fitness trainer"),
        new("Retail worker"),
        new("Waiter"),
        new("Computer systems administrator"),
        new("Database administrator"),
        new("Programmer"),
        new("Software developer"),
        new("Web developer")
    };

    public static JobType FromRequest(JobTypeRequest request)
    {
        if (request.Id == UnknownJobTypeId) return new(UnknownJobTypeId, request.Name);

        var matchedType = KnownJobTypes.FirstOrDefault(t => t.Id == request.Id);
        if (matchedType is null || !matchedType.Name.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase))
            return new(request.Id, request.Name);
        return matchedType;
    }

    public JobTypeResponse ToResponse()
    {
        return new()
        {
            Id = Id,
            Name = Name
        };
    }

    public bool IsValid()
    {
        if (Id == UnknownJobTypeId) return true;

        var matchedType = KnownJobTypes.FirstOrDefault(t => t.Id == Id);
        return matchedType is not null && matchedType.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase);
    }

    public bool IsKnown()
    {
        return Id != UnknownJobTypeId;
    }

    public static JobType GetById(int id)
    {
        if (id == UnknownJobTypeId) return UnknownJobType;
        return KnownJobTypes.FirstOrDefault(t => t.Id == id)
               ?? throw new InvalidOperationException($"There is no known job type with ID {id}");
    }

    public static IReadOnlyList<JobType> GetAll()
    {
        return new[] { UnknownJobType }.Concat(KnownJobTypes).ToList();
    }
}