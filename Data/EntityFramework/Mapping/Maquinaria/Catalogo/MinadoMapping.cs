using Core.Entity.Maquinaria.SOS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Catalogo
{
    public class MinadoMapping : EntityTypeConfiguration<MinadoEntity>
    {
        MinadoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Signum_Account_No).HasColumnName("Signum_Account_No");
            Property(x => x.Name).HasColumnName("Name");
            Property(x => x.IMO_No).HasColumnName("IMO_No");
            Property(x => x.Facility).HasColumnName("Facility");
            Property(x => x.City).HasColumnName("City");
            Property(x => x.Country).HasColumnName("Country");
            Property(x => x.Signum_Number).HasColumnName("Signum_Number");
            Property(x => x.Unit_ID).HasColumnName("Unit_ID");
            Property(x => x.Description).HasColumnName("Description");
            Property(x => x.Code).HasColumnName("Code");
            Property(x => x.Application).HasColumnName("Application");
            Property(x => x.Registered_Lubricant).HasColumnName("Registered_Lubricant");
            Property(x => x.Location).HasColumnName("Location");
            Property(x => x.Manufacturer).HasColumnName("Manufacturer");
            Property(x => x.Model).HasColumnName("Model");
            Property(x => x.Sample_Point_Status).HasColumnName("Sample_Point_Status");
            Property(x => x.Licensee).HasColumnName("Licensee");
            Property(x => x.Service_Type).HasColumnName("Service_Type");
            Property(x => x.Component_Type).HasColumnName("Component_Type");
            Property(x => x.Target_PC_4).HasColumnName("Target_PC_4");
            Property(x => x.Target_PC_6).HasColumnName("Target_PC_6");
            Property(x => x.Target_PC_14).HasColumnName("Target_PC_14");
            Property(x => x.Fuel_Type).HasColumnName("Fuel_Type");
            Property(x => x.RPM).HasColumnName("RPM");
            Property(x => x.Cycles).HasColumnName("Cycles");
            Property(x => x.Pressure).HasColumnName("Pressure");
            Property(x => x.kW_Rating).HasColumnName("kW_Rating");
            Property(x => x.Coolant_Type).HasColumnName("Coolant_Type");
            Property(x => x.Coolant_Brand).HasColumnName("Coolant_Brand");
            Property(x => x.Cylinder_Number).HasColumnName("Cylinder_Number");
            Property(x => x.Sample_ID).HasColumnName("Sample_ID");
            Property(x => x.Date_Sampled).HasColumnName("Date_Sampled");
            Property(x => x.Date_Received).HasColumnName("Date_Received");
            Property(x => x.Date_Reported).HasColumnName("Date_Reported");
            Property(x => x.Date_Landed).HasColumnName("Date_Landed");
            Property(x => x.Port_Landed).HasColumnName("Port_Landed");
            Property(x => x.Sampling_Point).HasColumnName("Sampling_Point");
            Property(x => x.Sampling_Point_Other).HasColumnName("Sampling_Point_Other");
            Property(x => x.Signum_Service).HasColumnName("Signum_Service");
            Property(x => x.Lubricant_Tested).HasColumnName("Lubricant_Tested");
            Property(x => x.Equipment_Age).HasColumnName("Equipment_Age");
            Property(x => x.Equipment_UOM).HasColumnName("Equipment_UOM");
            Property(x => x.Oil_Age).HasColumnName("Oil_Age");
            Property(x => x.Oil_Age_UOM).HasColumnName("Oil_Age_UOM");
            Property(x => x.Oil_Age_UOM_Status).HasColumnName("Oil_Age_UOM_Status");
            Property(x => x.Total_Engine_Hours).HasColumnName("Total_Engine_Hours");
            Property(x => x.Oil_Service_Hours).HasColumnName("Oil_Service_Hours");
            Property(x => x.Oil_Volume).HasColumnName("Oil_Volume");
            Property(x => x.Oil_Volume_UOM).HasColumnName("Oil_Volume_UOM");
            Property(x => x.Makeup_Volume).HasColumnName("Makeup_Volume");
            Property(x => x.MakeUp_Volume_UOM).HasColumnName("MakeUp_Volume_UOM");

            Property(x => x.Oil_Changed).HasColumnName("Oil_Changed");
            Property(x => x.Filter_Changed).HasColumnName("Filter_Changed");
            Property(x => x.Oil_Temp_In).HasColumnName("Oil_Temp_In");
            Property(x => x.Oil_Temp_Out).HasColumnName("Oil_Temp_Out");
            Property(x => x.Oil_Temp_UOM).HasColumnName("Oil_Temp_UOM");
            Property(x => x.Coolant_Temp_In).HasColumnName("Coolant_Temp_In");
            Property(x => x.Coolant_Temp_Out).HasColumnName("Coolant_Temp_Out");
            Property(x => x.Coolant_Temp_UOM).HasColumnName("Coolant_Temp_UOM");

            Property(x => x.Reservoir_Temp).HasColumnName("Reservoir_Temp");
            Property(x => x.Reservoir_Temp_UOM).HasColumnName("Reservoir_Temp_UOM");
            Property(x => x.Reservoir_Temp_UOM_Status).HasColumnName("Reservoir_Temp_UOM_Status");
            Property(x => x.Ambient_Temp).HasColumnName("Ambient_Temp");
            Property(x => x.Ambient_Temp_Status).HasColumnName("Ambient_Temp_Status");
            Property(x => x.Relative_Humidity).HasColumnName("Relative_Humidity");
            Property(x => x.Oil_Used_24_Hrs).HasColumnName("Oil_Used_24_Hrs");
            Property(x => x.Oil_Used_24_Hrs_UOM).HasColumnName("Oil_Used_24_Hrs_UOM");
            Property(x => x.Oil_Used_24_Hrs_UOM_Status).HasColumnName("Oil_Used_24_Hrs_UOM_Status");
            Property(x => x.kW_at_Sampling).HasColumnName("kW_at_Sampling");
            Property(x => x.kW_at_Sampling_Status).HasColumnName("kW_at_Sampling_Status");
            Property(x => x.Feed_Rate_l_cyd_day).HasColumnName("Feed_Rate_l_cyd_day");
            Property(x => x.Feed_Rate_l_cyd_day_Status).HasColumnName("Feed_Rate_l_cyd_day_Status");

            Property(x => x.Contamination_Rating).HasColumnName("Contamination_Rating");
            Property(x => x.Contamination_Rating_Status).HasColumnName("Contamination_Rating_Status");
            Property(x => x.Equipment_Rating).HasColumnName("Equipment_Rating");
            Property(x => x.Equipment_Rating_Status).HasColumnName("Equipment_Rating_Status");

            Property(x => x.Oil_Rating).HasColumnName("Oil_Rating");
            Property(x => x.Oil_Rating_Status).HasColumnName("Oil_Rating_Status");
            Property(x => x.Overall_Status).HasColumnName("Overall_Status");
            Property(x => x.Sample_Result).HasColumnName("Sample_Result");
            Property(x => x.Sample_Result_Status).HasColumnName("Sample_Result_Status");
            Property(x => x.Ag_Silver).HasColumnName("Ag_Silver");
            Property(x => x.Ag_Silver_Status).HasColumnName("Ag_Silver_Status");
            Property(x => x.Air_Release).HasColumnName("Air_Release");
            Property(x => x.Air_Release_Status).HasColumnName("Air_Release_Status");

            Property(x => x.Al_Aluminum).HasColumnName("Al_Aluminum");
            Property(x => x.Al_Aluminum_Status).HasColumnName("Al_Aluminum_Status");
            Property(x => x.Alcohol_wt_P).HasColumnName("Alcohol_wt_P");
            Property(x => x.Alcohol_wt_P_Status).HasColumnName("Alcohol_wt_P_Status");
            Property(x => x.Alkalinity_Reserve_wt_P).HasColumnName("Alkalinity_Reserve_wt_P");
            Property(x => x.Alkalinity_Reserve_wt_P_Status).HasColumnName("Alkalinity_Reserve_wt_P_Status");
            Property(x => x.Antioxidant_wt_P).HasColumnName("Antioxidant_wt_P");
            Property(x => x.Antioxidant_wt_P_Status).HasColumnName("Antioxidant_wt_P_Status");


            Property(x => x.Appearance).HasColumnName("Appearance");
            Property(x => x.Appearance_Status).HasColumnName("Appearance_Status");
            Property(x => x.B__Boron).HasColumnName("B__Boron");
            Property(x => x.B__Boron_Status).HasColumnName("B__Boron_Status");
            Property(x => x.Ba_Barium).HasColumnName("Ba_Barium");
            Property(x => x.Ba_Barium_Status).HasColumnName("Ba_Barium_Status");
            Property(x => x.Ca_Calcium).HasColumnName("Ca_Calcium");
            Property(x => x.Ca_Calcium_Status).HasColumnName("Ca_Calcium_Status");
            Property(x => x.Cd_Cadmium).HasColumnName("Cd_Cadmium");
            Property(x => x.Cd_Cadmium_Status).HasColumnName("Cd_Cadmium_Status");
            Property(x => x.Cl_Chlorine__Xray).HasColumnName("Cl_Chlorine__Xray");

            Property(x => x.Cl_Chlorine__Xray_Status).HasColumnName("Cl_Chlorine__Xray_Status");
            Property(x => x.Compatibility).HasColumnName("Compatibility");
            Property(x => x.Compatibility_Status).HasColumnName("Compatibility_Status");
            Property(x => x.Coolant_Indicator).HasColumnName("Coolant_Indicator");
            Property(x => x.Coolant_Indicator_Status).HasColumnName("Coolant_Indicator_Status");
            Property(x => x.Cr_Chromium).HasColumnName("Cr_Chromium");
            Property(x => x.Cr_Chromium_Status).HasColumnName("Cr_Chromium_Status");
            Property(x => x.Cu_Copper).HasColumnName("Cu_Copper");
            Property(x => x.Cu_Copper_Status).HasColumnName("Cu_Copper_Status");
            Property(x => x.DAC__P_Asphaltines).HasColumnName("DAC__P_Asphaltines");
            Property(x => x.DAC__P_Asphaltines_Status).HasColumnName("DAC__P_Asphaltines_Status");
            Property(x => x.Demul__a_54C).HasColumnName("Demul__a_54C");

            Property(x => x.Demul__a_54C_Status).HasColumnName("Demul__a_54C_Status");
            Property(x => x.Ester_wt_P).HasColumnName("Ester_wt_P");
            Property(x => x.Ester_wt_P_Status).HasColumnName("Ester_wt_P_Status");
            Property(x => x.Fe_Iron).HasColumnName("Fe_Iron");
            Property(x => x.Fe_Iron_Status).HasColumnName("Fe_Iron_Status");
            Property(x => x.Flash_Point_COC).HasColumnName("Flash_Point_COC");
            Property(x => x.Flash_Point_COC_Status).HasColumnName("Flash_Point_COC_Status");
            Property(x => x.Flash_Point_PM).HasColumnName("Flash_Point_PM");
            Property(x => x.Flash_Point_PM_Status).HasColumnName("Flash_Point_PM_Status");

            Property(x => x.Flash_Point_SETA).HasColumnName("Flash_Point_SETA");
            Property(x => x.Flash_Point_SETA_Status).HasColumnName("Flash_Point_SETA_Status");
            Property(x => x.Foam_Seq_1).HasColumnName("Foam_Seq_1");
            Property(x => x.Foam_Seq_1_Status).HasColumnName("Foam_Seq_1_Status");
            Property(x => x.Fuel_Dilut_Vol_P).HasColumnName("Fuel_Dilut_Vol_P");
            Property(x => x.Fuel_Dilut_Vol_P_Status).HasColumnName("Fuel_Dilut_Vol_P_Status");

            Property(x => x._P_Fuel_Sulfur).HasColumnName("_P_Fuel_Sulfur");
            Property(x => x._P_Fuel_Sulfur_Status).HasColumnName("_Fuel_Sulfur__Fuel_Sulfur_Status");
            Property(x => x.Initial_pH).HasColumnName("Initial_pH");
            Property(x => x.Initial_pH_Status).HasColumnName("Initial_pH_Status");
            Property(x => x.Insolubles_5u).HasColumnName("Insolubles_5u");
            Property(x => x.Insolubles_5u_Status).HasColumnName("Insolubles_5u_Status");
            Property(x => x.ISO_Code_4_6_14).HasColumnName("ISO_Code_4_6_14");
            Property(x => x.ISO_Code_4_6_14_Status).HasColumnName("ISO_Code_4_6_14_Status");
            Property(x => x.K_Potassium).HasColumnName("K_Potassium");
            Property(x => x.K_Potassium_Status).HasColumnName("K_Potassium_Status");
            Property(x => x.MCR).HasColumnName("MCR");
            Property(x => x.MCR_Status).HasColumnName("MCR_Status");
            Property(x => x.Mg__Magnesium).HasColumnName("Mg__Magnesium");
            Property(x => x.Mg__Magnesium_Status).HasColumnName("Mg__Magnesium_Status");

            Property(x => x.Mg_Magnesium).HasColumnName("Mg_Magnesium");
            Property(x => x.Mg_Magnesium_Status).HasColumnName("Mg_Magnesium_Status");

            Property(x => x.Mo_Molybdenum).HasColumnName("Mo_Molybdenum");
            Property(x => x.Mo_Molybdenum_Status).HasColumnName("Mo_Molybdenum_Status");
            Property(x => x.Na_Sodium).HasColumnName("Na_Sodium");
            Property(x => x.Na_Sodium_Status).HasColumnName("Na_Sodium_Status");

            Property(x => x.Ni_Nickel).HasColumnName("Ni_Nickel");
            Property(x => x.Ni_Nickel_Status).HasColumnName("Ni_Nickel_Status");
            Property(x => x.Nitration_Ab_cm).HasColumnName("Nitration_Ab_cm");
            Property(x => x.Nitration_Ab_cm_Status).HasColumnName("Nitration_Ab_cm_Status");
            Property(x => x.Nitration_Tendency).HasColumnName("Nitration_Tendency");

            Property(x => x.Nitration_Tendency_Status).HasColumnName("Nitration_Tendency_Status");
            Property(x => x.Nitrocmpnds_Ab_cm).HasColumnName("Nitrocmpnds_Ab_cm");
            Property(x => x.Nitrocmpnds_Ab_cm_Status).HasColumnName("Nitrocmpnds_Ab_cm_Status");
            Property(x => x.Oxidation_Ab_cm).HasColumnName("Oxidation_Ab_cm");
            Property(x => x.Oxidation_Ab_cm_Status).HasColumnName("Oxidation_Ab_cm_Status");
            Property(x => x.Oxidation_Tendency).HasColumnName("Oxidation_Tendency");
            Property(x => x.Oxidation_Tendency_Status).HasColumnName("Oxidation_Tendency_Status");

            Property(x => x.P_Phosphorus).HasColumnName("P_Phosphorus");
            Property(x => x.P_Phosphorus_Status).HasColumnName("P_Phosphorus_Status");
            Property(x => x.Particle_Count_14u).HasColumnName("Particle_Count_14u");
            Property(x => x.Particle_Count_14u_Status).HasColumnName("Particle_Count_14u_Status");
            Property(x => x.Particle_Count_4u).HasColumnName("Particle_Count_4u");
            Property(x => x.Particle_Count_4u_Status).HasColumnName("Particle_Count_4u_Status");
            Property(x => x.Particle_Count_6u).HasColumnName("Particle_Count_6u");
            Property(x => x.Particle_Count_6u_Status).HasColumnName("Particle_Count_6u_Status");
            Property(x => x.Pb_Lead).HasColumnName("Pb_Lead");
            Property(x => x.Pb_Lead_Status).HasColumnName("Pb_Lead_Status");
            Property(x => x.pH).HasColumnName("pH");
            Property(x => x.pH_Status).HasColumnName("pH_Status");
            Property(x => x.PQ__Index).HasColumnName("PQ__Index");
            Property(x => x.PQ__Index_Status).HasColumnName("PQ__Index_Status");
            Property(x => x.RBOT_Min).HasColumnName("RBOT_Min");
            Property(x => x.RBOT_Min_Status).HasColumnName("RBOT_Min_Status");
            Property(x => x.SAN).HasColumnName("SAN");
            Property(x => x.SAN_Status).HasColumnName("SAN_Status");
            Property(x => x.Si_Silicon).HasColumnName("Si_Silicon");
            Property(x => x.Si_Silicon_Status).HasColumnName("Si_Silicon_Status");
            Property(x => x.Sn_Tin).HasColumnName("Sn_Tin");
            Property(x => x.Sn_Tin_Status).HasColumnName("Sn_Tin_Status");
            Property(x => x.Soot_Wt_P).HasColumnName("Soot_Wt_P");
            Property(x => x.Soot_Wt_P_Status).HasColumnName("Soot_Wt_P_Status");
            Property(x => x.TBN_mg_KOH_gm).HasColumnName("TBN_mg_KOH_gm");
            Property(x => x.TBN_mg_KOH_gm_Status).HasColumnName("TBN_mg_KOH_gm_Status");
            Property(x => x.Ti_Titanium).HasColumnName("Ti_Titanium");
            Property(x => x.Ti_Titanium_Status).HasColumnName("Ti_Titanium_Status");

            Property(x => x.Ultra_Centrifuge_Rating).HasColumnName("Ultra_Centrifuge_Rating");
            Property(x => x.Ultra_Centrifuge_Rating_Status).HasColumnName("Ultra_Centrifuge_Rating_Status");
            Property(x => x.V_Vanadium).HasColumnName("V_Vanadium");
            Property(x => x.V_Vanadium_Status).HasColumnName("V_Vanadium_Status");
            Property(x => x.Viscosity__a_100C).HasColumnName("Viscosity__a_100C");
            Property(x => x.Viscosity__a_100C_Status).HasColumnName("Viscosity__a_100C_Status");
            Property(x => x.Viscosity__a_40C).HasColumnName("Viscosity__a_40C");
            Property(x => x.Viscosity__a_40C_Status).HasColumnName("Viscosity__a_40C_Status");

            Property(x => x.Viscosity_Grade).HasColumnName("Viscosity_Grade");
            Property(x => x.Viscosity_Grade_Status).HasColumnName("Viscosity_Grade_Status");
            Property(x => x.Water_Hot_Plate).HasColumnName("Water_Hot_Plate");
            Property(x => x.Water_Hot_Plate_Status).HasColumnName("Water_Hot_Plate_Status");
            Property(x => x.Water_Vol__P).HasColumnName("Water_Vol__P");
            Property(x => x.Water_Vol__P_Status).HasColumnName("Water_Vol__P_Status");
            Property(x => x.Water_Vol__P).HasColumnName("Water_Vol__P");
            Property(x => x.Water_Vol__P_Status).HasColumnName("Water_Vol__P_Status");

            Property(x => x.Zn__Zinc).HasColumnName("Zn__Zinc");
            Property(x => x.Zn__Zinc_Status).HasColumnName("Zn__Zinc_Status");
            Property(x => x.Zn_Zinc).HasColumnName("Zn_Zinc");
            Property(x => x.Comments).HasColumnName("Comments");
            Property(x => x.Comments_Status).HasColumnName("Comments_Status");
            Property(x => x.Demul_a54C_Oil_Water_Emul_Time).HasColumnName("Demul_a54C_Oil_Water_Emul_Time");
            Property(x => x.Demul_a54C_Oil_Water_Emul_Time_Status).HasColumnName("Demul_a54C_Oil_Water_Emul_Time_Status");
            Property(x => x.Demul_a54C_min).HasColumnName("Demul_a54C_min");
            Property(x => x.Demul_a54C_min_Status).HasColumnName("Demul_a54C_min_Status");
            Property(x => x.Particle_Count_Diluted_4um).HasColumnName("Particle_Count_Diluted_4um");
            Property(x => x.Particle_Count_Diluted_4um_Status).HasColumnName("Particle_Count_Diluted_4um_Status");
            Property(x => x.Particle_Count_Diluted_6um).HasColumnName("Particle_Count_Diluted_6um");
            Property(x => x.Particle_Count_Diluted_6um_Status).HasColumnName("Particle_Count_Diluted_6um_Status");
            Property(x => x.Particle_Count_Diluted_14um).HasColumnName("Particle_Count_Diluted_14um");
            Property(x => x.Particle_Count_Diluted_14um_Status).HasColumnName("Particle_Count_Diluted_14um_Status");
            Property(x => x.Diluted_ISO_Code_4_6_14).HasColumnName("Diluted_ISO_Code_4_6_14");
            Property(x => x.Diluted_ISO_Code_4_6_14_Status).HasColumnName("Diluted_ISO_Code_4_6_14_Status");
            Property(x => x.Mn_Manganese).HasColumnName("Mn_Manganese");
        }
    }
}
