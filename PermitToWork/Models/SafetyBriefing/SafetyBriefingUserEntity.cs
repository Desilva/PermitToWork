using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.SafetyBriefing
{
    public class SafetyBriefingUserEntity : safety_briefing_user
    {
        public override safety_briefing safety_briefing { get { return null; } set { } }
        public UserEntity userEntity { get; set; }

        private star_energy_ptwEntities db;

        public SafetyBriefingUserEntity()
            : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public SafetyBriefingUserEntity(int id, UserEntity user) : this()
        {
            safety_briefing_user safetyUser = this.db.safety_briefing_user.Find(id);
            // this.ptw = new PtwEntity(fi.id_ptw.Value);
            ModelUtilization.Clone(safetyUser, this);
            int userId = 0;
            if (this.user != null && Int32.TryParse(this.user, out userId))
            {
                this.userEntity = new UserEntity(userId, user.token, user);
            }
            else
            {
                this.userEntity = new UserEntity();
                this.userEntity.alpha_name = this.user;
            }
        }

        public SafetyBriefingUserEntity(int id_safety_briefing, string user, string id_badge)
            : this()
        {
            this.id_safety_briefing = id_safety_briefing;
            this.user = user;
            this.id_badge = id_badge;
        }

        public int create()
        {
            int retVal = 0;

            safety_briefing_user safetyUser = new safety_briefing_user();
            ModelUtilization.Clone(this, safetyUser);
            this.db.safety_briefing_user.Add(safetyUser);
            retVal = this.db.SaveChanges();

            this.id = safetyUser.id;

            return retVal;
        }

        public int edit()
        {
            int retVal = 0;
            safety_briefing_user safetyUser = this.db.safety_briefing_user.Find(this.id);
            if (safetyUser != null)
            {
                safetyUser.user = this.user;
                safetyUser.id_badge = this.id_badge;

                this.db.Entry(safetyUser).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int delete()
        {
            int retVal = 0;
            safety_briefing_user safetyUser = this.db.safety_briefing_user.Find(this.id);
            if (safetyUser != null)
            {
                this.db.safety_briefing_user.Remove(safetyUser);
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public List<SafetyBriefingUserEntity> listUser(int id_safety_briefing, UserEntity user)
        {
            List<int> listId = this.db.safety_briefing_user.Where(p => p.id_safety_briefing == id_safety_briefing).Select(p => p.id).ToList();
            List<SafetyBriefingUserEntity> listResult = new List<SafetyBriefingUserEntity>();
            foreach (int i in listId)
            {
                listResult.Add(new SafetyBriefingUserEntity(i, user));
            }

            return listResult;
        }
    }
}