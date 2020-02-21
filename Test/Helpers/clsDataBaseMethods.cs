using Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Helpers
{

    /// <summary>
    /// class to execute data operations for User
    /// </summary>
    public class clsDataBaseMethods
    {

        /// <summary>
        /// validate if the user exists by Id
        /// </summary>
        /// <param name="strId">Id of user</param>
        /// <returns>bool</returns>
        public bool ExistsId(string strId)
        {
            using (var context = new Entity.testRealEntities())
            {
                int idElemento = int.Parse(strId);
                var objPost = context.user.FirstOrDefault(p => p.Id == idElemento);

                if (objPost == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }


            }
        }

        /// <summary>
        /// validate if the login exists 
        /// </summary>
        /// <param name="strLogin">user name or login</param>
        /// <returns>bool</returns>
        public bool ExistsLogin(string strLogin)
        {
            using (var context = new Entity.testRealEntities())
            {
                //Se valida si ya existe
                var objUser = context.user.FirstOrDefault(d => d.login.ToUpper().Trim() == strLogin.ToUpper().Trim());
                if (objUser == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        /// <summary>
        /// Return a list of users
        /// </summary>
        /// <param name="blnAuthenticated">Param to identify if the user is authenticated</param>
        /// <returns></returns>
        public List<Models.user> ListUsers()
        {
            //Lista a retornar o mostrar en la grilla
            var Lista = new List<Models.user>();

            using (var context = new Entity.testRealEntities())
            {
                //Consulta a la tabla
                List<Entity.user> lstUsers;
                lstUsers = context.user.OrderBy(d => d.login).ToList();

                foreach (Entity.user item in lstUsers)
                {
                    //Se agregan a la lista
                    Lista.Add(new Models.user
                    {
                        UserName = item.login,
                        Age = (item.age == null ? "": item.age.ToString()),
                        Gender = item.gender,
                        Role = item.role,
                        Nationality = item.nationality,
                        IdUser = (int)item.Id
                    });
                }
                return Lista;
            }
        }

        /// <summary>
        /// delete method for user
        /// </summary>
        /// <param name="strId"></param>
        public void Delete(string strId)
        {
            using (var context = new Entity.testRealEntities())
            {
                int idElemento = int.Parse(strId);
                var objUser = context.user.FirstOrDefault(p => p.Id== idElemento);

                if (objUser == null)
                {
                    return;
                }

                context.user.Remove(objUser);
                context.SaveChanges();

            }
        }

        /// <summary>
        /// Method to insert new user
        /// </summary>
        /// <param name="objUser">Element with all data to save</param>
        public void Insert(Models.user objUser)
        {
            using (var context = new Entity.testRealEntities())
            {
                //create model and save to data base
                var model = new Entity.user();
                model.login = objUser.UserName;
                model.password = objUser.Password;
                model.gender = objUser.Gender;
                model.role = objUser.Role;
                model.nationality = objUser.Nationality;
                model.age = Convert.ToInt32(objUser.Age);
                context.user.Add(model);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Method to update a user
        /// </summary>
        /// <param name="objUser">Element with all data to save</param>
        public void Update(Models.user objUser)
        {
            using (var context = new Entity.testRealEntities())
            {
                //elemento a actualizar
                var update = context.user.FirstOrDefault(t => t.Id == objUser.IdUser);

                //Se actualiza
                update.password = objUser.Password.Trim();
                update.gender = objUser.Gender;
                update.role = objUser.Role;
                update.nationality = objUser.Nationality;
                update.age = Convert.ToInt32(objUser.Age);
                context.SaveChanges();
            }
        }
    }
}