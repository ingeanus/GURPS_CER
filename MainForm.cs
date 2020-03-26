using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace GURPS_CER
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Calculate_Affliction();
            Calculate_ActiveDefense();
            Calculate_AttackSkill();
            Calculate_Damage();
            Calculate_DamageResistence();
            Calculate_FatiguePoints();
            Calculate_Health();
            Calculate_HitPoints();
            Calculate_Move();
            Calculate_Will();
            Populate_Tooltips();
        }

        private void Populate_Tooltips()
        {
            this.mainFormTooltip.SetToolTip(this.drLabel2, "This is the amount of points spent on DR that only applies to special attacks, such as Fire Resistance.");
            this.mainFormTooltip.SetToolTip(this.multiAttackCheckbox, "This is only if you are capable of MULTIPLE Rapid Strike attacks that have a skill above 11.");
            this.mainFormTooltip.SetToolTip(this.rapidFireLabel, "This is the bonus for firing a large amount of attacks at once (B373), not the amount of shots you can fire.");
            this.mainFormTooltip.SetToolTip(this.conditionLabel, "This is the % that the condition would be worth as an enhancement to an Affliction, e.g. 300% for Heart Attack, so input 300.");
            this.mainFormTooltip.SetToolTip(this.afflictionAttackLabel, "This is the GURPS CER value for the damage of the attack tied to this Affliction. It's calculated the same as in the Damage tab, but they can be different attacks.");
            this.mainFormTooltip.SetToolTip(this.cyclesLabel, "This is the amount of cycles that take place within 15 seconds. For normal attacks that are not cyclic, this is 1.");
            this.mainFormTooltip.SetToolTip(this.parryMultipleCheckBox, "e.g. dual wielding or fighting unarmed where you can parry with both hands.");
            this.mainFormTooltip.SetToolTip(this.defenseAdvantagesLabel, "Injury Tolerance, Resistant, Supernatural Durability, Unkillable, and so on.");
            this.mainFormTooltip.SetToolTip(this.defenseDisadvantagesLabel, "Dependency, Fragile, Vulnerability, Weakness, etc.");
            this.mainFormTooltip.SetToolTip(this.hpAdvantagesLabel, "Rapid Healing, Recovery, Regeneration, etc.");
            this.mainFormTooltip.SetToolTip(this.curePointsLabel, "The % that the condition you can cure would be worth as an enhancement to an Affliction (e.g. 300% is a heart attack, so put 300 for an ability to rescuscitate from that).");
            this.mainFormTooltip.SetToolTip(this.saveButton, "Click to Save this character as a .gcer file.");
        }
        
        private void RangedCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (rangedCheckbox.Checked)
            {
                accuracyLabel.Visible = true;
                accuracyNumeric.Visible = true;
                rapidFireLabel.Visible = true;
                rapidFireNumeric.Visible = true;
            }
            else
            {
                accuracyLabel.Visible = false;
                accuracyNumeric.Visible = false;
                rapidFireLabel.Visible = false;
                rapidFireNumeric.Visible = false;
            }

            Calculate_AttackSkill();
        }

        private void RapidStrikesCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (rapidStrikesCheckbox.Checked)
            {
                rapidStrikesLabel.Visible = true;
                rapidStrikesNumeric.Visible = true;
                rapidStrikesSkillLabel.Visible = true;
                rapidStrikesSkillNumeric.Visible = true;
            }
            else
            {
                rapidStrikesLabel.Visible = false;
                rapidStrikesNumeric.Visible = false;
                rapidStrikesSkillLabel.Visible = false;
                rapidStrikesSkillNumeric.Visible = false;
            }

            Calculate_AttackSkill();
        }

        private void ParryMultipleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (parryMultipleCheckBox.Checked)
            {
                parryMultipleAdvantageCheckbox.Visible = true;
                parryMultipleLabel.Visible = true;
                parryMultipleNumeric.Visible = true;
            }
            else
            {
                parryMultipleAdvantageCheckbox.Visible = false;
                parryMultipleLabel.Visible = false;
                parryMultipleNumeric.Visible = false;
            }
            Calculate_ActiveDefense();
        }
        private void HealingAbilityCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (healingAbilityCheckbox.Checked)
            {
                healingLabel.Visible = true;
                healingLabel2.Visible = true;
                healingNumeric.Visible = true;
            }
            else
            {
                healingLabel.Visible = false;
                healingLabel2.Visible = false;
                healingNumeric.Visible = false;
            }
            Calculate_HitPoints();
        }

        private void cureCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (cureCheckbox.Checked)
            {
                curePointsLabel.Visible = true;
                curePointsNumeric.Visible = true;
            }
            else
            {
                curePointsLabel.Visible = true;
                curePointsNumeric.Visible = true;
            }
            Calculate_HitPoints();
        }
        
        private void conditionCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (conditionCheckbox.Checked)
            {
                conditionLabel.Visible = true;
                conditionNumeric.Visible = true;
            }
            else
            {
                conditionLabel.Visible = false;
                conditionNumeric.Visible = false;
            }

            Calculate_Affliction();
        }

        private void afflictionAttackCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (afflictionAttackCheckBox.Checked)
            {
                afflictionAttackLabel.Visible = true;
                afflictionAttackNumeric.Visible = true;
            }
            else
            {
                afflictionAttackLabel.Visible = false;
                afflictionAttackNumeric.Visible = false;
            }
            Calculate_Affliction();
        }

        private void frightCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (frightCheckbox.Checked)
            {
                frightLabel.Visible = true;
                frightNumeric.Visible = true;
            }
            else
            {
                frightLabel.Visible = false;
                frightNumeric.Visible = false;
            }
            Calculate_Affliction();
        }

        private void bindingCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (bindingCheckbox.Checked)
            {
                bindingLabel.Visible = true;
                bindingNumeric.Visible = true;
            }
            else
            {
                bindingLabel.Visible = false;
                bindingNumeric.Visible = false;
            }
            Calculate_Affliction();
        }

        private void Calculate_ActiveDefense()
        {
            decimal parryMultiple = 0;
            decimal blockAdvantage = 0;
            decimal dodgeTotal;
            decimal parryTotal;
            decimal blockTotal;

            if (parryMultipleCheckBox.Checked)
            {
                if (parryMultipleAdvantageCheckbox.Checked)
                {
                    parryMultiple = 2 * parryMultipleNumeric.Value + 1;
                }
                else
                {
                    parryMultiple = 2 * parryMultipleNumeric.Value;
                }
            }

            if (blockWeaponMasterCheckbox.Checked)
            {
                blockAdvantage = 1;
            }

            dodgeTotal = (dodgeNumeric.Value - 8) * 2;
            parryTotal = parryNumeric.Value - 8 + parryMultiple;
            blockTotal = blockNumeric.Value - 8 + blockAdvantage;

            activeDefenseTotal.Text = Math.Max(Math.Max(dodgeTotal, parryTotal), blockTotal).ToString();
            Calculate_Summary();
        }

        private void Calculate_DamageResistence()
        {
            decimal dr;
            decimal specDR;
            decimal hardenedLevels;
            decimal advantagePoints;
            decimal disadvantagePoints;
            decimal magicResistance;

            dr = (drhHeadNumeric.Value + drTorsoNumeric.Value + drArmsNumeric.Value + drLegsNumeric.Value) / 4;
            specDR = (pointsSpecHeadNumeric.Value + pointsSpecTorsoNumeric.Value + pointsSpecArmsNumeric.Value + pointsSpecLegsNumeric.Value) / 5;
            hardenedLevels = (hardenedHeadNumeric.Value + hardenedTorsoNumeric.Value + hardenedArmsNumeric.Value + hardenedLegsNumeric.Value) * 5;
            advantagePoints = defenseAdvantagesNumeric.Value / 5;
            disadvantagePoints = -(defenseDisadvantagesNumeric.Value) / 5;
            magicResistance = magicResistanceNumeric.Value;

            drTotal.Text = (dr + specDR + hardenedLevels + advantagePoints + disadvantagePoints + magicResistance).ToString();
            Calculate_Summary();
        }

        private void Calculate_Health()
        {
            decimal health;
            decimal hardTOs;
            decimal advantages = 0;

            if (highPainToleranceCheckbox.Checked)
            {
                advantages += 2;
            }
            if (recoveryCheckbox.Checked)
            {
                advantages += 2;
            }

            health = htNumeric.Value - 10;
            hardTOs = (hardToKillNumeric.Value + hardToSubdueNumeric.Value) / 2;

            healthTotal.Text = (health + hardTOs + advantages).ToString();
            Calculate_Summary();
        }

        private void Calculate_HitPoints()
        {
            decimal hitPoints;
            decimal healingPoints;
            decimal healingOrCure = 0;

            hitPoints = hitPointsNumber.Value - 10;
            healingPoints = hpAdvantagesNumeric.Value / 5;
            if (healingAbilityCheckbox.Checked || cureCheckbox.Checked)
            {
                healingOrCure = Math.Max(healingNumeric.Value / 2, curePointsNumeric.Value / 5);
            }

            hitPointsTotal.Text = (hitPoints + healingPoints + healingOrCure).ToString();
            Calculate_Summary();
        }

        private void Calculate_Will()
        {
            decimal will;
            decimal fearlessness;
            decimal advantages = 0;

            if (combatReflexesCheckbox.Checked)
            {
                advantages += 1;
            }
            if (unfazeableCheckbox.Checked)
            {
                advantages += 8;
            }

            will = willNumeric.Value - 10;
            fearlessness = fearlessnessNumeric.Value / 2;

            willTotal.Text = (will + fearlessness + advantages).ToString();
            Calculate_Summary();
        }

        private void Calculate_Damage()
        {
            decimal damage;
            decimal damageMult = 0;
            decimal rapidFireMult = 1;
            decimal divisor;

            if (rangedCheckbox.Checked)
            {
                rapidFireMult = rapidFireNumeric.Value + 1;
            }
            damage = damageNumeric.Value * rapidFireMult;

            if (damageTypeComboBox.Text.Equals("pi-"))
            {
                damageMult = 0.5m;
            }
            else if (damageTypeComboBox.Text.Equals("burn") || damageTypeComboBox.Text.Equals("cor") || damageTypeComboBox.Text.Equals("cr") || damageTypeComboBox.Text.Equals("fat") || damageTypeComboBox.Text.Equals("pi") || damageTypeComboBox.Text.Equals("tox"))
            {
                damageMult = 1;
            }
            else if (damageTypeComboBox.Text.Equals("cut") || damageTypeComboBox.Text.Equals("pi+"))
            {
                damageMult = 1.5m;
            }
            else if (damageTypeComboBox.Text.Equals("imp") || damageTypeComboBox.Text.Equals("pi++"))
            {
                damageMult = 2;
            }
            
            damage *= damageMult;

            if (explosiveCheckbox.Checked)
            {
                damage += 0.5m;
            }
            if (vampiricCheckBox.Checked)
            {
                damage += 1;
            }

            divisor = (armourDivisorCombobox.SelectedIndex - 3) * 5;

            damage += divisor;

            damage *= cyclesNumeric.Value;

            if (attackCostsFP.Checked)
            {
                damage /= 2;
            }

            damageTotal.Text = damage.ToString();
            Calculate_Summary();
        }

        private void Calculate_Affliction()
        {
            decimal affliction;
            decimal condition = 0;
            decimal fright = 0;
            decimal binding = 0;

            if (conditionCheckbox.Checked)
            {
                condition = conditionNumeric.Value / 5;
            }
            if (frightCheckbox.Checked)
            {
                fright = frightNumeric.Value / 5;
            }
            if (bindingCheckbox.Checked)
            {
                binding = bindingNumeric.Value;
            }

            affliction = condition + fright + binding;

            if (afflictionAttackCheckBox.Checked)
            {
                if (afflictionAttackNumeric.Value > affliction)
                {
                    affliction = affliction / 5 + afflictionAttackNumeric.Value;
                }
                else
                {
                    affliction = afflictionAttackNumeric.Value / 5 + affliction;
                }
            }

            if (afflictionCostsFP.Checked)
            {
                affliction /= 2;
            }

            afflictionTotal.Text = affliction.ToString();
            Calculate_Summary();
        }

        private void Calculate_AttackSkill()
        {
            decimal skill;
            decimal accuracy = 0;
            decimal multipleAttacks = 0;
            decimal rapidFire = 0;
            decimal advantages = 0;

            skill = attackSkilluNumeric.Value - 10;

            if (rangedCheckbox.Checked)
            {
                accuracy = accuracyNumeric.Value + 2;
                rapidFire = rapidFireNumeric.Value;
            }

            if (multiAttackCheckbox.Checked)
            {
                multipleAttacks = 5;
            }
            
            if (rapidStrikesCheckbox.Checked)
            {
                multipleAttacks = Math.Max(multipleAttacks, (rapidStrikesSkillNumeric.Value - 10) * (rapidStrikesNumeric.Value));
            }

            if (heroicArcherCheckbox.Checked)
            {
                advantages += 3;
            }
            if (trainedByAMasterCheckbox.Checked)
            {
                advantages += 3;
            }
            if (weaponMasterCheckbox.Checked)
            {
                advantages += 3;
            }
            if(automaticallyHitsCheckbox.Checked)
            {
                advantages += 15;
            }

            attackSkillTotal.Text = (skill + accuracy + multipleAttacks + rapidFire + advantages).ToString();
            Calculate_Summary();
        }

        private void Calculate_Move()
        {
            moveTotal.Text = (moveNumeric.Value - 6).ToString();
            Calculate_Summary();
        }

        private void Calculate_FatiguePoints()
        {
            decimal fp;
            decimal fpAdvantages;

            fp = (fatiguePointsNumeric.Value - 10) + fatiguePointsBonusNumeric.Value;
            fpAdvantages = fatigueRecoveryAdvantagesNumeric.Value / 5;

            fatiguePointsTotal.Text = (fp + fpAdvantages).ToString();
            Calculate_Summary();
        }

        private void Calculate_Summary()
        {
            decimal total;
            decimal offense;
            decimal defense;
            offense = Decimal.Parse(attackSkillTotal.Text) + Decimal.Parse(afflictionTotal.Text) + Decimal.Parse(damageTotal.Text) + Decimal.Parse(fatiguePointsTotal.Text) + Decimal.Parse(moveTotal.Text);
            defense = Decimal.Parse(activeDefenseTotal.Text) + Decimal.Parse(drTotal.Text) + Decimal.Parse(healthTotal.Text) + Decimal.Parse(hitPointsTotal.Text) + Decimal.Parse(willTotal.Text);
            total = offense + defense;

            summary.Text = total.ToString();
            offence.Text = offense.ToString();
            defence.Text = defense.ToString();
        }

        private void DodgeNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_ActiveDefense();
        }

        private void parryNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_ActiveDefense();
        }

        private void parryMultipleNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_ActiveDefense();
        }

        private void parryMultipleAdvantageCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Calculate_ActiveDefense();
        }

        private void blockNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_ActiveDefense();
        }

        private void blockWeaponMasterCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Calculate_ActiveDefense();
        }

        private void drhHeadNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_DamageResistence();
        }

        private void drArmsNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_DamageResistence();
        }

        private void drLegsNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_DamageResistence();
        }

        private void drTorsoNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_DamageResistence();
        }

        private void pointsSpecHeadNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_DamageResistence();
        }

        private void pointsSpecArmsNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_DamageResistence();
        }

        private void pointsSpecLegsNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_DamageResistence();
        }

        private void pointsSpecTorsoNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_DamageResistence();
        }

        private void hardenedHeadNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_DamageResistence();
        }

        private void hardenedArmsNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_DamageResistence();
        }

        private void hardenedLegsNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_DamageResistence();
        }

        private void hardenedTorsoNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_DamageResistence();
        }

        private void defenseAdvantagesNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_DamageResistence();
        }

        private void defenseDisadvantagesNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_DamageResistence();
        }

        private void magicResistanceNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_DamageResistence();
        }

        private void htNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_Health();
        }

        private void hardToKillNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_Health();
        }

        private void hardToSubdueNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_Health();
        }

        private void highPainToleranceCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Calculate_Health();
        }

        private void recoveryCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Calculate_Health();
        }

        private void hitPointsNumber_ValueChanged(object sender, EventArgs e)
        {
            Calculate_HitPoints();
        }

        private void hpAdvantagesNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_HitPoints();
        }

        private void HealingNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_HitPoints();
        }

        private void curePointsNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_HitPoints();
        }

        private void willNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_Will();
        }

        private void fearlessnessNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_Will();
        }

        private void combatReflexesCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Calculate_Will();
        }

        private void unfazeableCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Calculate_Will();
        }

        private void damageNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_Damage();
        }

        private void damageTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Calculate_Damage();
        }

        private void explosiveCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Calculate_Damage();
        }

        private void vampiricCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Calculate_Damage();
        }

        private void armourDivisorCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Calculate_Damage();
        }

        private void cyclesNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_Damage();
        }

        private void requiredFPCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Calculate_Damage();
        }

        private void conditionNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_Affliction();
        }

        private void afflictionAttackNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_Affliction();
        }

        private void frightNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_Affliction();
        }

        private void bindingNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_Affliction();
        }

        private void attackSkilluNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_AttackSkill();
        }

        private void accuracyNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_AttackSkill();
        }

        private void rapidFireNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_AttackSkill();
            Calculate_Damage();
        }

        private void rapidStrikesNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_AttackSkill();
        }

        private void multiAttackCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Calculate_AttackSkill();
        }

        private void rapidStrikesSkillNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_AttackSkill();
        }

        private void heroicArcherCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Calculate_AttackSkill();
        }

        private void trainedByAMasterCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Calculate_AttackSkill();
        }

        private void weaponMasterCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Calculate_AttackSkill();
        }

        private void automaticallyHitsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Calculate_AttackSkill();
        }

        private void afflictionCostsFP_CheckedChanged(object sender, EventArgs e)
        {
            Calculate_Affliction();
        }

        private void moveNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_Move();
        }

        private void fatigueRecoveryAdvantagesNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_FatiguePoints();
        }

        private void fatiguePointsNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_FatiguePoints();
        }

        private void fatiguePointsBonusNumeric_ValueChanged(object sender, EventArgs e)
        {
            Calculate_FatiguePoints();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "GURPS CER|*.gcer";
            saveFileDialog1.Title = "Save a GCER File";
            saveFileDialog1.FileName = "";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamWriter f_out = new System.IO.StreamWriter(saveFileDialog1.FileName);
                foreach (TabPage tab in attackTabControl.Controls)
                {
                    foreach (var item in tab.Controls)
                    {
                        if (item.GetType() == typeof(NumericUpDown))
                        {
                            NumericUpDown temp = (NumericUpDown)item;
                            f_out.WriteLine(temp.Value.ToString());
                        }
                        else if (item.GetType() == typeof(CheckBox))
                        {
                            CheckBox temp = (CheckBox)item;
                            f_out.WriteLine(temp.Checked.ToString());
                        }
                        else if (item.GetType() == typeof(ComboBox))
                        {
                            ComboBox temp = (ComboBox)item;
                            f_out.WriteLine(temp.Text.ToString());
                        }
                    }
                }

                foreach (TabPage tab in defenseTabControl.Controls)
                {
                    foreach (var item in tab.Controls)
                    {
                        if (item.GetType() == typeof(NumericUpDown))
                        {
                            NumericUpDown temp = (NumericUpDown)item;
                            f_out.WriteLine(temp.Value.ToString());
                        }
                        else if (item.GetType() == typeof(CheckBox))
                        {
                            CheckBox temp = (CheckBox)item;
                            f_out.WriteLine(temp.Checked.ToString());
                        }
                        else if (item.GetType() == typeof(ComboBox))
                        {
                            ComboBox temp = (ComboBox)item;
                            f_out.WriteLine(temp.Text.ToString());
                        }
                    }
                }
                f_out.Close();
            }
        }

        public void Load_File(string file)
        {
            System.IO.StreamReader f_in = new System.IO.StreamReader(file);
            string line;

            foreach (TabPage tab in attackTabControl.Controls)
            {
                foreach (var item in tab.Controls)
                {
                    if (item.GetType() == typeof(NumericUpDown))
                    {
                        line = f_in.ReadLine();
                        decimal temp = Decimal.Parse(line);
                        item.GetType().GetProperty("Value").SetValue(item, temp);
                    }
                    else if (item.GetType() == typeof(CheckBox))
                    {
                        line = f_in.ReadLine();
                        bool temp = Boolean.Parse(line);
                        item.GetType().GetProperty("Checked").SetValue(item, temp);
                    }
                    else if (item.GetType() == typeof(ComboBox))
                    {
                        line = f_in.ReadLine();
                        item.GetType().GetProperty("Text").SetValue(item, line);
                    }
                }
            }

            foreach (TabPage tab in defenseTabControl.Controls)
            {
                foreach (var item in tab.Controls)
                {
                    if (item.GetType() == typeof(NumericUpDown))
                    {
                        line = f_in.ReadLine();
                        decimal temp = Decimal.Parse(line);
                        item.GetType().GetProperty("Value").SetValue(item, temp);
                    }
                    else if (item.GetType() == typeof(CheckBox))
                    {
                        line = f_in.ReadLine();
                        bool temp = Boolean.Parse(line);
                        item.GetType().GetProperty("Checked").SetValue(item, temp);
                    }
                    else if (item.GetType() == typeof(ComboBox))
                    {
                        line = f_in.ReadLine();
                        item.GetType().GetProperty("Text").SetValue(item, line);
                    }
                }
            }
            f_in.Close();
        }

        private void loadButton2_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @".\Characters";
            openFileDialog1.FileName = "";

            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    this.Load_File(file);
                }
                catch (IOException)
                {
                }
            }
        }
    }
}
