using System.Net;

namespace VolleyLeague.Entities.Models
{
    public partial class User : BaseEntity
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public Guid? AccountId { get; set; }

        public int? BirthYear { get; set; }

        public string? City { get; set; }

        public string? PersonalInfo { get; set; }

        public byte[]? Photo { get; set; }

        public bool Gender { get; set; }

        public byte? Height { get; set; }

        public byte? Weight { get; set; }

        public byte? JerseyNumber { get; set; }

        public int? BlockRange { get; set; }

        public int? AttackRange { get; set; }

        public string? VolleyballIdol { get; set; }

        public string? AdditionalEmail { get; set; }

        public string? Hobby { get; set; }

        public string? Phone { get; set; }

        public int PositionId { get; set; }

        public int? PhotoWidth { get; set; }

        public int? PhotoHeight { get; set; }

        public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

        public virtual Team? Team { get; set; }

        public virtual ICollection<TeamPlayer> TeamPlayers { get; set; } = new List<TeamPlayer>();

        public virtual Position Position { get; set; } = null!;

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public virtual ICollection<PersonalLog> PersonalLogs { get; set; } = new List<PersonalLog>();

        public virtual ICollection<Match> MVPMatches { get; set; } = new List<Match>();

        public virtual ICollection<Match> RefereeMatches { get; set; } = new List<Match>();

        public virtual ICollection<ForumTopic> Topics { get; set; } = new List<ForumTopic>();
        public virtual ICollection<TypedResult> TypedResults { get; set; } = new List<TypedResult>();

        public virtual ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
        public virtual Credentials? Credentials { get; set; } = null!;
    }
}