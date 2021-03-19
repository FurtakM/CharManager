using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace CharManager
{
    class ModManager
    {
        public string path = String.Empty;
        public string campaignsPath = String.Empty;
        public string campaignActive = String.Empty;
        public List<string> campaings = new List<string>(5)
        {
            Campaign.AM,
            Campaign.RU,
            Campaign.AR,
            Campaign.X1,
            Campaign.X2
        };

        public int charactersCount;
        public List<Character> characters = new List<Character>();
        public List<Character> defaultCharacters = new List<Character>();

        public Regex regEnd = new Regex("(END)");
        public Regex regEndDefine = new Regex("(END_OF_DEFINE)");
        public Regex regEndPortret = new Regex("(END_OF_PORTRET)");
        public Regex regID = new Regex("([A-z]+[0-9]*) ([0-9]+)");
        public Regex regName = new Regex("(NAME) ([A-z]+[0-9]*)");
        public Regex regHuman = new Regex("(HUMAN) ([0-9]+) ([0-9]+) ([0-9]+)");
        public Regex regAttr = new Regex("(ATTR) ([0-9]+) ([0-9]+)");
        public Regex regSkills = new Regex("(SKILLS) ([0-9]{1,2}) ([0-9]+) ([0-9]{1,2}) ([0-9]+) ([0-9]{1,2}) ([0-9]+) ([0-9]{1,2}) ([0-9]+)");
        public Regex regFirstMission = new Regex("(LAST_MISSION) ([0-9]{1,2})");
        public Regex regImportance = new Regex("(CHAR) (@) ([0-9]{1,3}) ([0-9]{1,3})");
        public Regex regRandseed = new Regex("(RANDSEED) ([-/0-9]{1,11})");
        public Regex regGallery = new Regex("(VOICE) ([0-9]{1,3}) (GALLERY) ([A-z0-9]+) ([0-9]{1,3})");
        public Regex regPortret = new Regex("(VOICE) ([0-9]{1,3}) (PORTRET)");


        public ModManager(string path)
        {
            this.path = path;
        }

        internal List<String> GetCharactersList()
        {
            if (this.charactersCount == 0)
                return new List<string>();

            List<string> list = new List<string>();

            for (int i = 0; i < this.characters.Count; i++)
            {
                list.Add(this.characters.ElementAt(i).GetID());
            }

            return list;
        }

        internal bool ParseCampaigns()
        {   
            string relativePath = "\\campaings\\";

            if (!Directory.Exists(this.path + relativePath))
                relativePath = "\\Campaigns\\";

            if (!Directory.Exists(this.path + relativePath))
                return false;

            this.campaignsPath = this.path + relativePath;

            for (int i = this.campaings.Count - 1; i >= 0; i--)
            {
                var campaign = this.campaings.ElementAt(i);

                if (!File.Exists(this.campaignsPath + campaign + "\\missions.dat"))
                    this.campaings.Remove(campaign);                    
            }

            return this.campaings.Count > 0;
        }

        internal bool LoadCampaign(int index)
        {
            if (this.campaings.Count == 0)
                return false;

            return this.ParseStartFile(index);
        }

        internal void RestoreDefault()
        {
            this.characters = new List<Character>(this.defaultCharacters);
        }

        private bool ParseStartFile(int index)
        {
            string filePath = this.campaignsPath + this.campaings.ElementAt(index) + "\\start.txt";

            if (!File.Exists(filePath))
                return false;

            this.campaignActive = this.campaignsPath + this.campaings.ElementAt(index);
            this.charactersCount = 0;
            this.characters = new List<Character>();
            this.defaultCharacters = new List<Character>();

            IEnumerable<string> file = File.ReadLines(filePath);
            IEnumerator<string> lines = file.GetEnumerator();

            for (int i = 0; lines.MoveNext(); i++)
            {
                var line = lines.Current;

                if (regEnd.IsMatch(line))
                    break;

                if (i == 0)
                    continue; // no support for variables

                if (i == 1)
                {
                    Regex reg = new Regex("^(CHARACTERS) ([0-9]+)$");

                    if (!reg.IsMatch(line))
                        return false;

                    string count = reg.Match(line).Groups[2].Value;

                    if (!Int32.TryParse(count, out this.charactersCount))
                        return false;
                }

                /*
                  Abdul 1
                    DEFINE
                      NAME Abdul
                      HUMAN 1 11 2
                      ATTR 10 10
                      SKILLS 1 48661 1 25425 1 44101 1 64091
                      LAST_MISSION 1
                      CHAR @ 20 0
                      RANDSEED 0
                      VOICE 3 GALLERY ru 45
                    END_OF_DEFINE
                */

                if (i > 1)
                {
                    Character character = new Character();

                    if (!regID.IsMatch(line))
                        return false;

                    string id = regID.Match(line).Groups[1].Value;

                    if (id.Length == 0)
                        return false;

                    character.SetID(id);

                    int characterCompLength = 0;

                    while (!regEndDefine.IsMatch(line))
                    {
                        lines.MoveNext();
                        i++;

                        characterCompLength++;

                        if (characterCompLength > 150)
                            return false;

                        line = lines.Current;

                        if (regName.IsMatch(line))
                        {
                            character.SetName(regName.Match(line).Groups[2].Value);
                        }

                        if (regHuman.IsMatch(line))
                        {
                            int sex, profession, nation;

                            if (!Int32.TryParse(regHuman.Match(line).Groups[2].Value, out sex))
                                return false;

                            if (!Int32.TryParse(regHuman.Match(line).Groups[3].Value, out profession))
                                return false;

                            if (!Int32.TryParse(regHuman.Match(line).Groups[4].Value, out nation))
                                return false;

                            character.SetSex(sex);
                            character.SetProfession(profession);
                            character.SetNation(nation);
                        }

                        if (regAttr.IsMatch(line))
                        {
                            int speed, armor;

                            if (!Int32.TryParse(regAttr.Match(line).Groups[2].Value, out armor))
                                return false;

                            if (!Int32.TryParse(regAttr.Match(line).Groups[3].Value, out speed))
                                return false;

                            character.SetAttr(armor, speed);
                        }

                        if (regSkills.IsMatch(line))
                        {
                            int[] skills = new int[4];
                            int[] exp = new int[4];

                            if (!Int32.TryParse(regSkills.Match(line).Groups[2].Value, out skills[0]))
                                return false;

                            if (!Int32.TryParse(regSkills.Match(line).Groups[4].Value, out skills[1]))
                                return false;

                            if (!Int32.TryParse(regSkills.Match(line).Groups[6].Value, out skills[2]))
                                return false;

                            if (!Int32.TryParse(regSkills.Match(line).Groups[8].Value, out skills[3]))
                                return false;

                            if (!Int32.TryParse(regSkills.Match(line).Groups[3].Value, out exp[0]))
                                return false;

                            if (!Int32.TryParse(regSkills.Match(line).Groups[5].Value, out exp[1]))
                                return false;

                            if (!Int32.TryParse(regSkills.Match(line).Groups[7].Value, out exp[2]))
                                return false;

                            if (!Int32.TryParse(regSkills.Match(line).Groups[9].Value, out exp[3]))
                                return false;

                            character.SetSkills(skills, exp);
                        }

                        if (regFirstMission.IsMatch(line))
                        {
                            int firstMission;

                            if (!Int32.TryParse(regFirstMission.Match(line).Groups[2].Value, out firstMission))
                                return false;

                            character.SetFirstMission(firstMission);
                        }

                        if (regImportance.IsMatch(line))
                        {
                            int importance;

                            if (!Int32.TryParse(regImportance.Match(line).Groups[3].Value, out importance))
                                return false;

                            character.SetImportance(importance);
                        }

                        if (regRandseed.IsMatch(line))
                        {
                            int randseed;

                            if (!Int32.TryParse(regRandseed.Match(line).Groups[2].Value, out randseed))
                                return false;

                            character.SetRandseed(randseed);
                        }

                        if (regGallery.IsMatch(line))
                        {
                            int faceNumber;
                            int voice;

                            if (!Int32.TryParse(regGallery.Match(line).Groups[2].Value, out voice))
                                return false;

                            character.SetVoice(voice);

                            if (!Int32.TryParse(regGallery.Match(line).Groups[5].Value, out faceNumber))
                                return false;

                            character.SetGallery(regGallery.Match(line).Groups[4].Value, faceNumber);
                        }
                        else if (regPortret.IsMatch(line))
                        {
                            int voice;

                            if (!Int32.TryParse(regPortret.Match(line).Groups[2].Value, out voice))
                                return false;

                            character.SetVoice(voice);

                            int portretCompLength = 0;

                            while (!regEndPortret.IsMatch(line))
                            {
                                lines.MoveNext();
                                i++;

                                line = lines.Current;
                                character.AddToPortret(line);

                                portretCompLength++;

                                if (portretCompLength > 120)
                                    return false;
                            }
                        }
                    }

                    this.characters.Add(character);
                    this.defaultCharacters.Add(new Character(character));
                }
            }

            lines.Dispose();

            return true;
        }

        internal void RestoreCurrentCharacterDefault(int selectedIndex)
        {
            if (this.characters.Count == 0 || this.defaultCharacters.Count <= selectedIndex)
                return;

            this.characters[selectedIndex] = this.defaultCharacters[selectedIndex];
        }

        internal bool Save()
        {
            string startTxt = this.campaignActive + "\\start.txt";

            if (startTxt.Length == 0 || !File.Exists(startTxt))
                return false;

            File.Copy(startTxt, this.campaignActive + "\\old_Start.txt", true);

            List<string> lines = new List<string>();

            lines.Add("VARIABLES 0");
            lines.Add("CHARACTERS " + this.charactersCount.ToString());

                
            /**
                Belkov 1
                DEFINE
                    NAME Belkov
                    HUMAN 1 1 3
                    ATTR 10 10
                    SKILLS 1 66633 1 17274 1 8843 0 23464
                    LAST_MISSION 1
                    CHAR @ 20 0
                    RANDSEED 0
                    VOICE 202 GALLERY ru 3
                END_OF_DEFINE
            */
            for (int j = 0; j < this.characters.Count; j++)
            {
                lines.Add("  " + this.characters[j].GetID() + " 1");
                lines.Add("    " + "DEFINE");
                lines.Add("      " + "NAME " + this.characters[j].GetName());
                lines.Add("      " + "HUMAN " + this.characters[j].GetSex().ToString() + " " + this.characters[j].GetProfession().ToString() + " " + this.characters[j].GetNation().ToString());
                lines.Add("      " + "ATTR " + this.characters[j].GetArmor().ToString() + " " + this.characters[j].GetSpeed().ToString());
                lines.Add("      " + "SKILLS " + this.characters[j].GetBasicSkill(0).ToString() + " " + (this.characters[j].GetExp(0) * 30).ToString() + " " 
                                               + this.characters[j].GetBasicSkill(1).ToString() + " " + (this.characters[j].GetExp(1) * 30).ToString() + " " 
                                               + this.characters[j].GetBasicSkill(2).ToString() + " " + (this.characters[j].GetExp(2) * 30).ToString() + " " 
                                               + this.characters[j].GetBasicSkill(3).ToString() + " " + (this.characters[j].GetExp(3) * 30).ToString());
                lines.Add("      " + "LAST_MISSION " + this.characters[j].GetFirstMission().ToString());
                lines.Add("      " + "CHAR @ " + this.characters[j].GetImportance().ToString() + " 0");
                lines.Add("      " + "RANDSEED " + this.characters[j].GetRandseed().ToString());

                if (this.characters[j].GetPortret().Count > 0)
                {
                    lines.Add("      " + "VOICE " + this.characters[j].GetVoice().ToString() + " PORTRET");

                    var portret = this.characters[j].GetPortret();

                    for (int i = 0; i < portret.Count; i++)
                    {
                        lines.Add(portret[i]);
                    }
                }
                else
                {
                    lines.Add("      " + "VOICE " + this.characters[j].GetVoice().ToString() + " GALLERY " + this.characters[j].GetGallery() + " " + this.characters[j].GetFaceNumber().ToString());
                }

                lines.Add("    " + "END_OF_DEFINE");
            }

            lines.Add("END");      
               
            File.WriteAllLines(startTxt, lines);

            return true;
        }

        internal void Delete(int index)
        {
            this.characters.RemoveAt(index);
            this.charactersCount = this.characters.Count;
        }

        internal bool Add(string id)
        {
            if (id.Length == 0)
                return false;

            for (int i = 0; i < this.charactersCount; i++)
            {
                if (this.characters[i].GetID() == id)
                    return false;
            }

            Character character = new Character(id);

            character.SetName(id);
            character.SetArmor(10);
            character.SetSpeed(10);
            character.SetSex(1);
            character.SetProfession(1);
            character.SetImportance(0);
            character.SetSkills(new int[4]{0, 0, 0, 0}, new int[4]{ 0, 0, 0, 0});
            character.SetGallery("", 0);
            character.SetFirstMission(1);
            character.SetRandseed(0);
            character.SetVoice(1);

            this.characters.Add(character);

            this.charactersCount = this.characters.Count;

            return true;
        }
    }
}
