using Sara.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sara.Data.Seeds
{
    public class IdentityDataInitializer
    {
        static Guid level1 = new Guid("d02613d3-9781-41ac-9dba-f2a125077099");
        static Guid level2 = new Guid("7324ce7c-1107-43e4-bfb9-4028b5edf79c");
        static Guid level3 = new Guid("ff7b1015-2643-4a3b-ba2e-1968d577e823");
        static Guid level4 = new Guid("e3950344-ce12-4c80-bfde-2259da3b0217");

        static Guid hetero = new Guid("ea4c3dbe-e08c-41d8-93b2-9bbc919efa51");
        static Guid homo = new Guid("a51d235e-0d2c-449a-ae69-b6762b7bbe63");
        static Guid bi = new Guid("b0f83b86-e3a5-4385-8f9b-785e6f6d89c6");
        static Guid pan = new Guid("3c6d62e6-be5b-4139-9c8e-18951bcc2985");
        static Guid demi = new Guid("86941a32-2018-409c-9acf-32f301c27e9a");
        static Guid assexual = new Guid("1373fa03-5b24-4459-9970-284a5be5612d");
        
        static Guid birth = new Guid("e63dc153-a579-4280-b860-be9fdcf64518");
        static Guid trans = new Guid("d84365fe-4407-4ee1-8e55-200c6ed56976");
        static Guid androgen = new Guid("928cee33-e32c-4fd9-aceb-b1615cf90c74");
        static Guid nonbinary = new Guid("300bbdbc-53eb-4941-97dd-5a997829f46d");
        static Guid floating = new Guid("483edd01-6303-448c-816c-70d67b8c5d22");

        static Guid classA = new Guid("a73e46e3-d854-4245-87ea-371c9b50c309");
        static Guid classB = new Guid("453c1b44-42d3-47bc-a4ca-b0fde2fe86d8");
        static Guid classC = new Guid("19334b72-b673-4e88-b440-fb76035a7962");
        static Guid classD = new Guid("3b88680c-61aa-49e2-87f8-f4a968147b42");

        public static void SeedData(SaraContext context)
        {
            if (!context.EducationLevels.Any(x => x.Id == level1))
            {
                context.Add(new EducationLevel(level1) { Description = "Ensino Fundamental" });
                context.Add(new EducationLevel(level2) { Description = "Ensino Médio" });
                context.Add(new EducationLevel(level3) { Description = "Ensino Superior" });
                context.Add(new EducationLevel(level4) { Description = "Pós-graduação" });

                context.Add(new Sexuality(hetero) { Description = "Heterosexual" });
                context.Add(new Sexuality(homo) { Description = "Homosexual" });
                context.Add(new Sexuality(bi) { Description = "Bissexual" });
                context.Add(new Sexuality(pan) { Description = "Pansexual" });
                context.Add(new Sexuality(demi) { Description = "Demisexual" });
                context.Add(new Sexuality(assexual) { Description = "Assexual" });

                context.Add(new GenderIdentity(birth) { Description = "Gênero de Nascimento" });
                context.Add(new GenderIdentity(trans) { Description = "Transgênero" });
                context.Add(new GenderIdentity(androgen) { Description = "Andrógeno" });
                context.Add(new GenderIdentity(nonbinary) { Description = "Não binário" });
                context.Add(new GenderIdentity(floating) { Description = "Gênero Flutuante" });

                context.Add(new Income(classA) { Description = "Até R$ 1.045", Min = 0, Max = 1045 });
                context.Add(new Income(classB) { Description = "De R$ 1.046 até R$ 5.000", Min = 1046, Max = 5000 });
                context.Add(new Income(classC) { Description = "De R$ 5.001 até R$ 10.000", Min = 5001, Max = 10000 });
                context.Add(new Income(classD) { Description = "Acima de R$ 10.001", Min = 10001, Max = int.MaxValue });

                context.SaveChanges();
            }
        }
    }
}
