using MyAPI.Models;
namespace MyAPI.Repository
{
    public interface IVisitorRepository
    {
        public Task<IEnumerable<Visitor>> GetAllVisitors();
        public Task<Visitor> GetVisitor(string hash);
        public Task<Visitor> InsertVisitor(Visitor visitor);
        public Task<int> UpdateVisitor(int id, Visitor visitor);
        public Task<int> DeleteVisitorById(int id);


    }
}
