﻿using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Fullstack_capstone.Models;
using Fullstack_capstone.Utils;

namespace Fullstack_capstone.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

        public UserProfile GetByFirebaseUserId(string firebaseUserId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT up.Id, Up.FirebaseUserId, up.FullName, up.DisplayName, 
                               up.Email, up.PrimaryFocusId, up.Image, up.Description,
                               pf.Name AS PrimaryFocus
                          FROM UserProfile up
                               LEFT JOIN PrimaryFocus pf on up.PrimaryFocusId = pf.Id
                         WHERE FirebaseUserId = @FirebaseuserId ";

                    DbUtils.AddParameter(cmd, "@FirebaseUserId", firebaseUserId);

                    UserProfile userProfile = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            FirebaseUserId = DbUtils.GetString(reader, "FirebaseUserId"),
                            FullName = DbUtils.GetString(reader, "FullName"),
                            DisplayName = DbUtils.GetString(reader, "DisplayName"),
                            Email = DbUtils.GetString(reader, "Email"),
                            
                           
                            PrimaryFocusId = DbUtils.GetInt(reader, "PrimaryFocusId"),
                            PrimaryFocus = new PrimaryFocus()
                            {
                                Id = DbUtils.GetInt(reader, "PrimaryFocusId"),
                                Name = DbUtils.GetString(reader, "PrimaryFocus"),
                            },

                             Image = DbUtils.GetString(reader, "Image"),
                             Description = DbUtils.GetString(reader, "Description")
                        };
                    }
                    reader.Close();

                    return userProfile;
                }
            }
        }

        public void Add(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO UserProfile (FirebaseUserId, FullName, DisplayName, 
                                                                 Email,   PrimaryFocusId, Image)
                                        OUTPUT INSERTED.ID
                                        VALUES (@FirebaseUserId, @Fullname, @DisplayName, 
                                                @Email,  @PrimaryFocusId, @Image)";
                    DbUtils.AddParameter(cmd, "@FirebaseUserId", userProfile.FirebaseUserId);
                    DbUtils.AddParameter(cmd, "@FullName", userProfile.FullName);
             
                    DbUtils.AddParameter(cmd, "@DisplayName", userProfile.DisplayName);
                    DbUtils.AddParameter(cmd, "@Email", userProfile.Email);
                   
                    DbUtils.AddParameter(cmd, "@ImageLocation", userProfile.Image);
                    DbUtils.AddParameter(cmd, "@UserTypeId", userProfile.PrimaryFocusId);

                    userProfile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public List<UserProfile> GetAllUserProfiles()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.Id, u.FirebaseUserId, u.FullName, u.DisplayName, u.Email,
                          u.UserTypeId, u.Image
                              pf.[Name] AS PrimaryFocus
                         FROM UserProfile u
                              LEFT JOIN PrimaryFocus pf ON u.UserTypeId = pf.Id
              
                        ORDER BY u.DisplayName;
                      
                       ";


                    List<UserProfile> userProfiles = new List<UserProfile>();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        userProfiles.Add(new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirebaseUserId = DbUtils.GetString(reader, "FirebaseUserId"),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            
                            FullName = reader.GetString(reader.GetOrdinal("FullName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            
                            Image = DbUtils.GetNullableString(reader, "Image"),
                            PrimaryFocusId = reader.GetInt32(reader.GetOrdinal("PrimaryFocusId")),
                            PrimaryFocus= new PrimaryFocus()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("PrimaryFocusId")),
                                Name = reader.GetString(reader.GetOrdinal("PrimaryFocus"))
                            },
                            
                        });
                    }

                    reader.Close();

                    return userProfiles;
                }
            }
        }


        public UserProfile GetUserProfileById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FullName, u.FirebaseUserId, u.DisplayName, u.Email,
                           u.Image, u.PrimaryFocusId,
                              pf.[Name] AS PrimaryFocus
                         FROM UserProfile u
                              LEFT JOIN PrimaryFocus pf ON u.PrimaryFocusId = pf.id
                        WHERE u.id = @id 
                        ORDER BY u.DisplayName;
                        
                       ";

                    cmd.Parameters.AddWithValue("@id", id);
                    UserProfile userProfile = new UserProfile();
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirebaseUserId = DbUtils.GetString(reader, "FirebaseUserId"),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FullName = reader.GetString(reader.GetOrdinal("FullName")),
                            
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                           
                            Image = DbUtils.GetNullableString(reader, "Image"),
                            PrimaryFocusId = reader.GetInt32(reader.GetOrdinal("PrimaryFocusId")),
                            PrimaryFocus = new PrimaryFocus()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("PrimaryFocusId")),
                                Name = reader.GetString(reader.GetOrdinal("PrimaryFocus"))
                            },
                            
                        };
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }
      


       
        



    }
}