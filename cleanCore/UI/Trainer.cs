using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cleanCore.UI
{
    public static class Trainer
    {
        public static void TrainAll()
        {
            WoWScript.ExecuteNoResults("LoadAddOn\"Blizzard_TrainerUI\" f=ClassTrainerTrainButton f.e = 0 if f:GetScript\"OnUpdate\" then f:SetScript(\"OnUpdate\", nil)else f:SetScript(\"OnUpdate\", function(f,e) f.e=f.e+e if f.e>.01 then f.e=0 f:Click() end end)end");
        }
    }
}
