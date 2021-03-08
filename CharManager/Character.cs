using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CharManager
{
    class Character
    {
        private static int[] expLevel = new int[10]
        {
            1000,
            3000,
            7000,
            11000,
            20000,
            35000,
            55000,
            90000,
            150000,
            240000
        };
        private string id;
        private string name;
        private int nation;
        private int profession;
        private int sex;
        private int[] basicSkills;
        private int[] skills;
        private int[] exp;
        private int firstMission;
        private int importance;
        private int randseed;
        private int voice;
        private string gallery;
        private int faceNumber;
        private int armor;
        private int speed;
        private List<string> portret;


        public Character()
        {
            this.portret = new List<string>();
        }


        public Character(string id)
        {
            this.id = id;
            this.portret = new List<string>();
        }

        public Character(Character character)
        {
            this.id = character.id;
            this.name = character.name;
            this.nation = character.nation;
            this.profession = character.profession;
            this.sex = character.sex;
            this.basicSkills = character.basicSkills;
            this.skills = character.skills;
            this.exp = character.exp;
            this.firstMission = character.firstMission;
            this.importance = character.importance;
            this.randseed = character.randseed;
            this.voice = character.voice;
            this.gallery = character.gallery;
            this.faceNumber = character.faceNumber;
            this.armor = character.armor;
            this.speed = character.speed;
            this.portret = character.portret;
        }

        public void AddToPortret(string phrase)
        {
            this.portret.Add(phrase);
        }

        public void ClearPortret()
        {
            this.portret.Clear();
        }

        public void SetID(string id)
        {
            this.id = id;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetNation(int nation)
        {
            this.nation = nation;
        }

        public void SetSex(int sex)
        {
            this.sex = sex;
        }

        public void SetProfession(int profession)
        {
            this.profession = profession;
        }

        public void SetBasicSkill(int skill, int value)
        {
            this.basicSkills[skill] = value;
        }

        public int SetSkill(int skill)
        {
            int exp = this.exp[skill];
            this.skills[skill] = 0;

            for (int j = 9; j >= 0; j--)
                if (Character.expLevel[j] <= exp)
                {
                    this.skills[skill] = this.basicSkills[skill] + (j + 1);
                    break;
                }

            if (this.skills[skill] < this.basicSkills[skill])
                this.skills[skill] = this.basicSkills[skill];

            if (this.skills[skill] > 10)
                this.skills[skill] = 10;

            return this.skills[skill];
        }

        public void SetExp(int exp, int value)
        {
            this.exp[exp] = value;
        }

        public void SetSkills(int[] _skills, int[] _exp)
        {
            this.basicSkills = _skills;
            this.skills = new int[4] { 0, 0, 0, 0 };
            this.exp = new int[4] { 0, 0, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 9; j >= 0; j--)
                    if (expLevel[j] <= (_exp[i] / 30))
                    {
                        this.skills[i] = this.basicSkills[i] + (j + 1);
                        break;
                    }

                if (this.skills[i] < this.basicSkills[i])
                    this.skills[i] = this.basicSkills[i];

                if (this.skills[i] > 10)
                    this.skills[i] = 10;

                this.exp[i] = _exp[i] / 30;
            }
        }

        public void SetFirstMission(int firstMission)
        {
            this.firstMission = firstMission;
        }

        public void SetImportance(int importance)
        {
            this.importance = importance;
        }

        public void SetRandseed(int randseed)
        {
            this.randseed = randseed;
        }

        public void SetVoice(int voice)
        {
            this.voice = voice;
        }

        public void SetGallery(string gallery, int faceNumber)
        {
            this.gallery = gallery;
            this.faceNumber = faceNumber;
        }

        public void SetSpeed(int speed)
        {
            this.speed = speed;
        }

        public void SetArmor(int armor)
        {
            this.armor = armor;
        }

        public void SetAttr(int armor, int speed)
        {
            this.armor = armor;
            this.speed = speed;
        }

        public string GetID()
        {
            return this.id;
        }

        public List<string> GetPortret()
        {
            return this.portret;
        }

        public int[] GetSkills()
        {
            return this.skills;
        }

        public int GetSkill(int skill)
        {
            return this.skills[skill];
        }

        public int[] GetBasicSkills()
        {
            return this.basicSkills;
        }

        public int GetBasicSkill(int skill)
        {
            return this.basicSkills[skill];
        }

        public int GetExp(int skill)
        {
            return this.exp[skill];
        }

        public string GetName()
        {
            return this.name;
        }

        public int GetSpeed()
        {
            return this.speed;
        }

        public int GetArmor()
        {
            return this.armor;
        }

        public int GetVoice()
        {
            return this.voice;
        }

        public string GetGallery()
        {
            return this.gallery;
        }

        public int GetNation()
        {
            return this.nation;
        }

        public int GetSex()
        {
            return this.sex;
        }

        public int GetFaceNumber()
        {
            return this.faceNumber;
        }

        public int GetRandseed()
        {
            return this.randseed;
        }

        public int GetProfession()
        {
            return this.profession;
        }

        public int GetFirstMission()
        {
            return this.firstMission;
        }

        public int GetImportance()
        {
            return this.importance;
        }

        public static int GetExpLevel(int level)
        {
            return expLevel[level];
        }

        internal void Compile()
        {
            string id = this.GetID();

            // name
            string name = this.GetName();

            if (name.Length < 3)
                throw new Exception(id + ": Name must have more than 2 characters.");

            if (name.Length > 16)
                throw new Exception(id + ": Name is too long. Maxium length is: 16.");

            // exp
            for (int i = 0; i < 3; i++)
                if (this.GetExp(i) > 200000)
                    throw new Exception(id + ": amount of experience for " + (i + 1) + " skill is too high!");

            // profession
            int[] allowedClass = new int[8] { 1, 2, 3, 4, 5, 8, 9, 11 };
            bool hasValidClass = false;

            for (int i = 0; i < 8; i++)
                if (this.GetProfession() == allowedClass[i])
                {
                    hasValidClass = true;
                    break;
                }

            if (!hasValidClass)
                throw new Exception(id + ": has invalid class!");

            if (this.GetPortret().Count == 0)
            {
                if (this.GetGallery() == String.Empty || this.GetFaceNumber() == 0)
                    throw new Exception(id + ": no avatar set!");
            }
        }
    }
}
