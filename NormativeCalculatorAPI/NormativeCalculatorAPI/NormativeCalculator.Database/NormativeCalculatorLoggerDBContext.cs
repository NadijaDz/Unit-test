using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NormativeCalculator.Database
{
    public class NormativeCalculatorLoggerDBContext: DbContext
    {
        public NormativeCalculatorLoggerDBContext(DbContextOptions<NormativeCalculatorLoggerDBContext> options) : base(options)
        {
        }
    }
}
