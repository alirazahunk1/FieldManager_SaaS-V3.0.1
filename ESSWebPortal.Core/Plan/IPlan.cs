using ESSWebPortal.Core.ViewModel.Plan;

namespace ESSWebPortal.Core.Plan
{
    public interface IPlan
    {
        Task<string> GetPlanDetailsForUser(string userId);

        Task<List<PlanViewModel>> GetAll();

        Task<PlanViewModel> GetById(int id);

        Task<bool> CreatePlan(PlanCreateUpdateVM vm);

        Task<bool> EditPlan(PlanCreateUpdateVM vm);

        Task<bool> DeletePlan(int id);

        Task ChangeStatus(int id);
    }
}
