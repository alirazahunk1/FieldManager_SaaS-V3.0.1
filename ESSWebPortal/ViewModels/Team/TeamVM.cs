using ESSDataAccess.Enum;

namespace ESSWebPortal.ViewModels.Team
{
    public class TeamVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int UserCount { get; set; }

        public TeamStatus Status { get; set; }

    }
}
