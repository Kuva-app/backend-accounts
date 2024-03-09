using System;
using System.Collections.Generic;
using Kuva.Accounts.Entities;
using RandomTestValues;

namespace Kuva.Accounts.Tests.Fixtures
{
    public static class UserFixture
    {
        private static List<UserEntity>? _usersUnregistered;
        private static List<UserEntity>? _usersRegistered;
        private static List<UserEntity>? _userRegisterThrowException;

        public static List<UserEntity> GetUnregisteredUsers()
        {
            return _usersUnregistered ??= new List<UserEntity>()
            {
                {
                    new()
                    {
                        Active = RandomValue.Bool(),
                        Email = "nairolu@meswe.bw",
                        Name = "Essie Gilbert",
                        Passcode = "Sgf0CiqR607x",
                        CreateAt = RandomValue.DateTime(DateTime.Now.AddYears(-5), DateTime.Now),
                        UserLevelId = UserLevels.Administrator
                    }
                },
                {
                    new()
                    {
                        Active = RandomValue.Bool(),
                        Email = "ogupino@jelkuzu.do",
                        Name = "Danny Ramos",
                        Passcode = "aMAsScCT",
                        CreateAt = RandomValue.DateTime(DateTime.Now.AddYears(-5), DateTime.Now),
                        UserLevelId = UserLevels.User
                    }
                },
                {
                    new()
                    {
                        Active = RandomValue.Bool(),
                        Email = "befvoro@secom.com",
                        Name = "Sarah Houston",
                        Passcode = "LN9yEip4VbGw",
                        CreateAt = RandomValue.DateTime(DateTime.Now.AddYears(-5), DateTime.Now),
                        UserLevelId = UserLevels.Client
                    }
                },
                {
                    new()
                    {
                        Active = RandomValue.Bool(),
                        Email = "ohi@dum.io",
                        Name = "Amanda Caldwell",
                        Passcode = "6972534746513408",
                        CreateAt = RandomValue.DateTime(DateTime.Now.AddYears(-5), DateTime.Now),
                        UserLevelId = UserLevels.Guest
                    }
                },
                {
                    new()
                    {
                        Active = RandomValue.Bool(),
                        Email = "uva@femege.aw",
                        Name = "Eva Joseph",
                        Passcode = "Czech Republic",
                        CreateAt = RandomValue.DateTime(DateTime.Now.AddYears(-5), DateTime.Now),
                        UserLevelId = UserLevels.User
                    }
                },
                {
                    new()
                    {
                        Active = RandomValue.Bool(),
                        Email = "voh@ogve.py",
                        Name = "Lillian Douglas",
                        Passcode = "vkGBrNVsSO",
                        CreateAt = RandomValue.DateTime(DateTime.Now.AddYears(-5), DateTime.Now),
                        UserLevelId = UserLevels.User
                    }
                },
                {
                    new()
                    {
                        Active = RandomValue.Bool(),
                        Email = "ep@luvuteg.lt",
                        Name = "Juan Jefferson",
                        Passcode = "VYikLDq8BkX0",
                        CreateAt = RandomValue.DateTime(DateTime.Now.AddYears(-5), DateTime.Now),
                        UserLevelId = UserLevels.User
                    }
                },
                {
                    new()
                    {
                        Active = RandomValue.Bool(),
                        Email = "rewa@vem.tl",
                        Name = "Christian Hernandez",
                        Passcode = "WncUsdLb",
                        CreateAt = RandomValue.DateTime(DateTime.Now.AddYears(-5), DateTime.Now),
                        UserLevelId = UserLevels.Administrator
                    }
                },
                {
                    new()
                    {
                        Active = RandomValue.Bool(),
                        Email = "kikiz@lib.tw",
                        Name = "Kenneth Hansen",
                        Passcode = "39.89.32.80",
                        CreateAt = RandomValue.DateTime(DateTime.Now.AddYears(-5), DateTime.Now),
                        UserLevelId = UserLevels.Guest
                    }
                },
                {
                    new()
                    {
                        Active = RandomValue.Bool(),
                        Email = "lolukzi@nib.kg",
                        Name = "Ophelia Tyler",
                        Passcode = "qW0x4ukXotSSVy",
                        CreateAt = RandomValue.DateTime(DateTime.Now.AddYears(-5), DateTime.Now),
                        UserLevelId = UserLevels.Administrator
                    }
                }
            };
        }

        public static List<UserEntity> GetRegisteredUsers()
        {
            return _usersRegistered ??= new List<UserEntity>()
            {
                {
                    new()
                    {
                        Id = 1,
                        Active = RandomValue.Bool(),
                        Email = "ul@uhnel.ch",
                        Name = "Gabriel Douglas",
                        Passcode = "DLNLajoBzNiJZV",
                        CreateAt = RandomValue.DateTime(DateTime.Now.AddYears(-5), DateTime.Now),
                        UserLevelId = UserLevels.Administrator
                    }
                },
                {
                    new()
                    {
                        Id = 2,
                        Active = RandomValue.Bool(),
                        Email = "ricce@laca.cu",
                        Name = "Brett Marshall",
                        Passcode = "qvjMpIbVFYhHzlC",
                        CreateAt = RandomValue.DateTime(DateTime.Now.AddYears(-5), DateTime.Now),
                        UserLevelId = UserLevels.User
                    }
                },
                {
                    new()
                    {
                        Id = 3,
                        Active = RandomValue.Bool(),
                        Email = "hah@lobtenwe.gd",
                        Name = "Isabel Greer",
                        Passcode = "LhXmyftZ",
                        CreateAt = RandomValue.DateTime(DateTime.Now.AddYears(-5), DateTime.Now),
                        UserLevelId = UserLevels.Client
                    }
                },
                new()
                {
                    Id = 4,
                    Active = RandomValue.Bool(),
                    Email = "ken@kilibciv.pw",
                    Name = "Jesse Mitchell",
                    Passcode = "q5Pt8MCN3rz9oLmy",
                    CreateAt = RandomValue.DateTime(DateTime.Now.AddYears(-5), DateTime.Now),
                    UserLevelId = UserLevels.Guest
                }
            };
        }

        public static UserEntity GetUserForUpdate() =>
            new()
            {
                Active = RandomValue.Bool(),
                Email = "nairolu@meswe.bw",
                Name = "Georgie Delgado",
                Passcode = "i5g0ov7s",
                CreateAt = RandomValue.DateTime(DateTime.Now.AddYears(-5), DateTime.Now),
                UserLevelId = UserLevels.Administrator
            };

        public static List<UserEntity> GetUserRegisterThrowException()
        {
            return _userRegisterThrowException ??= new List<UserEntity>
            {
                {
                    new()
                    {
                        Email = null,
                        Active = false,
                        CreateAt = default,
                        Id = default,
                        Name = null,
                        Passcode = null,
                        UserLevelId = UserLevels.Administrator
                    }
                },
                {
                    new()
                    {
                        Email = "user.domain.com",
                        Active = false,
                        CreateAt = default,
                        Id = default,
                        Name = null,
                        Passcode = null,
                        UserLevelId = UserLevels.User
                    }
                }
            };
        }
    }
}