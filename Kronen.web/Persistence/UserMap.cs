using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kronen.web.Persistence
{
    public class UserMap
    {
        
         public UserMap(EntityTypeBuilder<User> entityBuilder)
        {
            entityBuilder.HasKey(t => new {t.Id});
            entityBuilder.Property(t => t.Id);
            entityBuilder.Property(t => t.Name);
        }
    }
}