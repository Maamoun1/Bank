using DataAccessLayer.Respository.IRepository;


namespace DataAccessLayer.Respository
{
    public class PersonRepository : GenericRepository<TbPerson>, IPersonRepostitory
    {

        private readonly DbContext.ApplicationDbContext _context;

        public PersonRepository(DbContext.ApplicationDbContext context) : base(context)
        {
            _context = context;
            
        }

        public bool IsPersonExist(int PersonID)
        {
          //  return _context.People.Find(PersonID) != null;
            return _context.People.Where(p => p.PersonId == PersonID).Any();
        }

        public void Update(TbPerson entity)
        {
            _context.People.Update(entity);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

    }
}
