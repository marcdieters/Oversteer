using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oversteer.Validators;

namespace Oversteer.Models
{
    public class League
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? OwnerId { get; set; }
        public string? AnonymousVisitorDesc { get; set; }
        public string? LoggedInUserDesc { get; set; }
        public bool OpenJoin { get; set; } = true;
        public bool AutoApproveNewMembers { get; set; } = true;
        public bool AcceptNewMembers { get; set; } = true;
        public bool AllowPremiumMembers { get; set; } = false;
        public string? BannerColor { get; set; }
        public string? PrimaryColor { get; set; }
        public string? SelectedColor { get; set; }
        public string? HeaderTextColor { get; set; }
        public string? URL { get; set; }
        public string? Logo { get; set; }
        public string? BackGrounndImage { get; set; }
        [DataType(DataType.Password)]
        public string? DefaultServerAdminPassword { get; set; }
        public string? FacebookUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? YoutubeUrl { get; set; }
        public string? DiscordUrl { get; set; }
        public string? PaypalKey { get; set; }
        public string? EmailAddress { get; set; }
        public string? TwitchUrl { get; set; }
        public int CustomLiveryFileSizeLimit { get; set; }
        public string? ReplayFilesLocation { get; set; }
        public int MaxNrOfConcurrentServers { get; set; }
        public int MaxNrOfConcurrentServersConnections { get; set; }
        public string? DiscordNewUserMessage { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
        public bool Approved { get; set; }
        public string? ApprovedByUserId { get; set; }
        public string? ApproveComment { get; set; }
        [Required(ErrorMessage = "A plan needs to be selected")]
        [NonEmptyGuid]
        public Guid PlanId { get; set; }
        public List<LeagueUser> LeagueUsers { get; set; } = new List<LeagueUser>();
        public List<Host> Hosts { get; set; } = new List<Host>();
    }

    public enum LeagueOrder
    {
        Name,
        LastUpdate,
        LastResult,
        NrOfMembers,
        NrOfActiveEvents
    }
}
