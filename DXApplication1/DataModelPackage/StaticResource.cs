using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXApplication1.DataModelPackage
{
  public   class StaticResource
    {
        
      // variable Type
        public static string multiFactorStr = "Multiple Factor";
        public static string singleFactorStr = "Single Factor";

        public static Dictionary<VariableType?, string> VariableTypeDict = new Dictionary<VariableType?, string>
        {
            {
                VariableType.MULTI_FACTOR,multiFactorStr
            },
            {
                VariableType.SINGLE_FACTOR,singleFactorStr
            }
        };

        //Priority DataTable
        public static string varialbeTypeStr = "Variable Type";
        public static string variableStr = "Variable";
        public static string platformStr = "Platform";


       //strategy
        public static string defaultStrategyNameStr = "Default";
        public static string defaultStrategDescripStr = "default setting";

      //load excel
        public static string prioritySheetNameStr = "Priority";
        public static string projectNameFlagStr = "Project Name";
        public static string variableNameFlagStr = "Variable Name";

       //analysis strategy form tree node name
        public static string strategyGeneralNodeStr ="General";
        public static string strategyPlatformWeightNodeStr= "Platform Weight";
        public static string strategyAssignMethodNodeStr ="Platform Assign Method";
        public static string strategyManualAssignNodeStr ="Manual Assignment";
        public static string strategyAutoAssignNodeStr = "Auto Assignment";
        public static string strategyPrimaryVariableManualAssignNodeStr ="Primary Variable";
        public static string strategySecondaryVariableManualAssignNodeStr ="Secondary Variable";
        public static string strategyFactorPriorityNodeNodeStr ="Factor Priority";
        public static string strategyPlatformPriorityNodeManualAssignNodeStr = "Platform Priority";
        
      // testSet grid column
        public static string testSetIterationStr = "Iteration";
        public static string testSetFactorStr = "Factor";

      // coverage grid column
        public static string coverageVariableTypeStr ="Variable Type";
        public static string coverageVariableStr = "Variable";
        public static string coveragePlatformStr = "Platform";
        public static string coverageSumStr="SUM";
        
        public static string coveragePrimarySingleTypeStr= "Primary Single Factor  Variable";
        public static string coveragePrimaryMultiTypeStr="Primary Multiple Factor Variable";
        public static string coverageSecondarySingleTypeStr = "Secondary Single Factor Variable";
        public static string coverageSecondaryMultiTypeStr="Secondary Multiple Factor Variable";
        //factor priority 
        public static string factorNameStr = "Factor Name";
        public static string factorPriorityStr = "Priority";

        //excel sheet name
        public static string testSetSheetFlagName = "TestSet";
        public static string coverageSheetFlagName = "Coverage";
        public static string testsetSheetSlogonStr = "TestSet Result:";
        public static string coverageSheetSlogonStr = "Coverage Check :";
    }

}
