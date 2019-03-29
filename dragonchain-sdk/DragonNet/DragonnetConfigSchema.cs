using dragonchain_sdk.Framework.Errors;

namespace dragonchain_sdk.DragonNet
{
    public class DragonnetConfigSchema
    {
        public decimal? L2 { get; set; }
        public decimal? L3 { get; set; }
        public decimal? L4 { get; set; }
        public decimal? L5 { get; set; }
    }

    public static class DragonnetConfigSchemaExtensions
    {
        public static Dragonnet ToDragonnet(this DragonnetConfigSchema schema)
        {
            ValidateSchema(schema);
            var dragonnet = new Dragonnet();
            if (schema.L2 != null) { dragonnet.L2 = new DragonnetPrice { MaximumPrice = (decimal)schema.L2 }; }
            if (schema.L3 != null) { dragonnet.L3 = new DragonnetPrice { MaximumPrice = (decimal)schema.L3 }; }
            if (schema.L4 != null) { dragonnet.L4 = new DragonnetPrice { MaximumPrice = (decimal)schema.L4 }; }
            if (schema.L5 != null) { dragonnet.L5 = new DragonnetPrice { MaximumPrice = (decimal)schema.L5 }; }
            return dragonnet;
        }

        private static void ValidateSchema(this DragonnetConfigSchema schema)
        {
            if (schema.L2 != null) { ValidateLevelPrice((decimal)schema.L2); }
            if (schema.L3 != null) { ValidateLevelPrice((decimal)schema.L3); }
            if (schema.L4 != null) { ValidateLevelPrice((decimal)schema.L4); }
            if (schema.L5 != null) { ValidateLevelPrice((decimal)schema.L5); }
        }

        private static void ValidateLevelPrice(decimal levelPrice)
        {
            if(levelPrice < 0 || levelPrice > 1000)
            {
                throw new FailureByDesignException("BAD_REQUEST", "maxPrice must be between 0 and 1000.");
            }
        }
    }
}