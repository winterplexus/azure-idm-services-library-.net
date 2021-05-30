//
//  ManageGroups.cs
//
//  Copyright (c) Wiregrass Code Technology 2020-2021
//
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace IdentityManagement.Services.Console.Application
{
    internal class ManageGroups
    {
        private readonly IIdentityManager identityManager;

        internal ManageGroups(IIdentityManager identityManager)
        {
            this.identityManager = identityManager;
        }

        internal void GetGroupByGroupName()
        {
            try
            {
                var groupName = ReadGroupName();

                var group = Task.Run(async () => await identityManager.GroupServices.GetGroupByGroupName(groupName).ConfigureAwait(false)).Result;

                WriteGroup(group);

                var groupMembers = Task.Run(async () => await identityManager.GroupServices.GetGroupMembers(groupName).ConfigureAwait(false)).Result;

                WriteGroupMembers(groupMembers);

                var groupOwners = Task.Run(async () => await identityManager.GroupServices.GetGroupOwners(groupName).ConfigureAwait(false)).Result;

                WriteGroupOwners(groupOwners);

                ReadContinue();
            }
            catch (Exception ex)
            {
                WriteException(ex);

                ReadContinue(false);
            }
        }

        internal void GetGroupByObjectId()
        {
            try
            {
                var groupObjectId = ReadGroupObjectId();

                var group = Task.Run(async () => await identityManager.GroupServices.GetGroupByObjectId(groupObjectId).ConfigureAwait(false)).Result;

                WriteGroup(group);

                var groupMembers = Task.Run(async () => await identityManager.GroupServices.GetGroupMembers(group.DisplayName).ConfigureAwait(false)).Result;

                WriteGroupMembers(groupMembers);

                var groupOwners = Task.Run(async () => await identityManager.GroupServices.GetGroupOwners(group.DisplayName).ConfigureAwait(false)).Result;

                WriteGroupOwners(groupOwners);

                ReadContinue();
            }
            catch (Exception ex)
            {
                WriteException(ex);

                ReadContinue(false);
            }
        }

        internal void GetGroups()
        {
            var limit = ReadLimit();

            try
            {
                var groupsList = Task.Run(async () => await identityManager.GroupServices.GetGroups(limit).ConfigureAwait(false)).Result;
                if (groupsList == null)
                {
                    return;
                }

                var counter = 0;
                foreach (var group in groupsList)
                {
                    WriteGroup(group);

                    var groupMembers = Task.Run(async () => await identityManager.GroupServices.GetGroupMembers(group.DisplayName).ConfigureAwait(false)).Result;

                    WriteGroupMembers(groupMembers);

                    var groupOwners = Task.Run(async () => await identityManager.GroupServices.GetGroupOwners(group.DisplayName).ConfigureAwait(false)).Result;

                    WriteGroupOwners(groupOwners);

                    counter++;
                }

                WriteRecordCount(counter);

                ReadContinue();
            }
            catch (Exception ex)
            {
                WriteException(ex);

                ReadContinue(false);
            }
        }

        internal void GetGroupsByName()
        {
            var groupName = ReadGroupName();

            try
            {
                var groupsList = Task.Run(async () => await identityManager.GroupServices.GetGroupsByName(groupName).ConfigureAwait(false)).Result;
                if (groupsList == null)
                {
                    return;
                }

                var counter = 0;
                foreach (var group in groupsList)
                {
                    WriteGroup(group);

                    var groupMembers = Task.Run(async () => await identityManager.GroupServices.GetGroupMembers(group.DisplayName).ConfigureAwait(false)).Result;

                    WriteGroupMembers(groupMembers);

                    var groupOwners = Task.Run(async () => await identityManager.GroupServices.GetGroupOwners(group.DisplayName).ConfigureAwait(false)).Result;

                    WriteGroupOwners(groupOwners);

                    counter++;
                }

                WriteRecordCount(counter);

                ReadContinue();
            }
            catch (Exception ex)
            {
                WriteException(ex);

                ReadContinue(false);
            }
        }

        internal void CreateGroup()
        {
            try
            {
                var groupModel = ReadGroup();

                var group = Task.Run(async () => await identityManager.GroupServices.GetGroupByGroupName(groupModel.DisplayName).ConfigureAwait(false)).Result;
                if (group == null)
                {
                    var groupObjectId = Task.Run(async () => await identityManager.GroupServices.CreateGroup(groupModel).ConfigureAwait(false)).Result;

                    WriteGroupObjectIdentifier(groupObjectId);
                }
                else
                {
                    System.Console.WriteLine($"{Environment.NewLine}error-> display name already exists (choose another group display name)");
                }

                ReadContinue();
            }
            catch (Exception ex)
            {
                WriteException(ex);

                ReadContinue(false);
            }
        }

        internal void DeleteGroup()
        {
            try
            {
                var groupName = ReadGroupName();

                var status = Task.Run(async () => await identityManager.GroupServices.DeleteGroup(groupName).ConfigureAwait(false)).Result;

                ReadContinue(status);
            }
            catch (Exception ex)
            {
                WriteException(ex);

                ReadContinue(false);
            }
        }

        internal void AddMemberToGroup()
        {
            try
            {
                var groupName = ReadGroupName();
                var userName = ReadUserName();

                var status = Task.Run(async () => await identityManager.GroupServices.AddMemberToGroup(groupName, userName).ConfigureAwait(false)).Result;

                ReadContinue(status);
            }
            catch (Exception ex)
            {
                WriteException(ex);

                ReadContinue(false);
            }
        }

        internal void RemoveMemberToGroup()
        {
            try
            {
                var groupName = ReadGroupName();
                var userName = ReadUserName();

                var status = Task.Run(async () => await identityManager.GroupServices.RemoveMemberFromGroup(groupName, userName).ConfigureAwait(false)).Result;

                ReadContinue(status);
            }
            catch (Exception ex)
            {
                WriteException(ex);

                ReadContinue(false);
            }
        }

        internal void AddOwnerToGroup()
        {
            try
            {
                var groupName = ReadGroupName();
                var userName = ReadUserName();

                var status = Task.Run(async () => await identityManager.GroupServices.AddOwnerToGroup(groupName, userName).ConfigureAwait(false)).Result;

                ReadContinue(status);
            }
            catch (Exception ex)
            {
                WriteException(ex);

                ReadContinue(false);
            }
        }

        internal void RemoveOwnerToGroup()
        {
            try
            {
                var groupName = ReadGroupName();
                var userName = ReadUserName();

                var status = Task.Run(async () => await identityManager.GroupServices.RemoveOwnerFromGroup(groupName, userName).ConfigureAwait(false)).Result;

                ReadContinue(status);
            }
            catch (Exception ex)
            {
                WriteException(ex);

                ReadContinue(false);
            }
        }

        private static void WriteGroup(GroupModel groupModel)
        {
            if (groupModel == null)
            {
                System.Console.WriteLine($"{Environment.NewLine}information-> unable to locate group");
                return;
            }

            System.Console.WriteLine("");
            System.Console.WriteLine("[ GROUP ]");
            System.Console.WriteLine($"- object ID            = {groupModel.Id}");
            System.Console.WriteLine($"- security enabled     = {groupModel.SecurityEnabled}");
            System.Console.WriteLine($"- created date time    = {groupModel.CreatedDateTime}");
            System.Console.WriteLine($"- display name         = {groupModel.DisplayName}");
            System.Console.WriteLine($"- description          = {groupModel.Description}");
        }

        private static void WriteGroupObjectIdentifier(string groupObjectId)
        {
            System.Console.WriteLine("");
            System.Console.WriteLine($"- group object ID      = {groupObjectId}");
        }

        private static void WriteGroupMembers(IList<UserModel> groupMembers)
        {
            if (groupMembers == null)
            {
                return;
            }

            System.Console.WriteLine($"                       = group members count     > {groupMembers.Count}");
            foreach (var member in groupMembers)
            {
                System.Console.WriteLine($"- member               = user object ID          > {member.Id}");
                System.Console.WriteLine($"-                      = user display name       > {member.DisplayName}");
            }
        }

        private static void WriteGroupOwners(IList<UserModel> groupOwners)
        {
            if (groupOwners == null)
            {
                return;
            }

            System.Console.WriteLine($"                       = group owners count      > {groupOwners.Count}");
            foreach (var member in groupOwners)
            {
                System.Console.WriteLine($"- owner                = user object ID          > {member.Id}");
                System.Console.WriteLine($"-                      = user display name       > {member.DisplayName}");
            }
        }

        private static void WriteRecordCount(int counter)
        {
            System.Console.WriteLine("");
            System.Console.WriteLine($"total records: {counter}");
        }

        private static void WriteException(Exception ex)
        {
            System.Console.WriteLine($"exception-> {ex.Message}");
            System.Console.WriteLine($"inner exception-> {ex.InnerException?.Message}");
            System.Console.WriteLine($"stack trace-> {Environment.NewLine}{ex.StackTrace}");
        }

        private static void ReadContinue()
        {
            System.Console.WriteLine("");
            System.Console.Write("PRESS ENTER TO CONTINUE ->");
            System.Console.ReadKey();
        }

        private static void ReadContinue(bool? status)
        {
            const string message = "PRESS ENTER TO CONTINUE ->";

            System.Console.WriteLine("");
            System.Console.Write(status != null ? $"{message}({status}) " : "{message}");
            System.Console.ReadKey();
        }

        private static int ReadLimit()
        {
            System.Console.WriteLine("");
            System.Console.Write("- limit (top groups)   : ");

            var line = System.Console.ReadLine();

            return !int.TryParse(line, out var number) ? 0 : number;
        }

        private static string ReadUserName()
        {
            System.Console.WriteLine("");
            System.Console.Write("- user name            : ");

            return System.Console.ReadLine();
        }

        private static string ReadGroupName()
        {
            System.Console.WriteLine("");
            System.Console.Write("- group name           : ");

            return System.Console.ReadLine();
        }

        private static string ReadGroupObjectId()
        {
            System.Console.WriteLine("");
            System.Console.Write("- group object ID      : ");

            return System.Console.ReadLine();
        }

        private static GroupModel ReadGroup()
        {
            var groupModel = new GroupModel
            {
                DisplayName = GetInputField("display name", 20),
                Description = GetInputField("description", 20)
            };

            return groupModel;
        }

        private static string GetInputField(string fieldLabel, int fieldLabelWidth)
        {
            if (string.IsNullOrEmpty(fieldLabel))
            {
                throw new ArgumentNullException(nameof(fieldLabel));
            }

            var width = fieldLabelWidth.ToString(CultureInfo.CurrentCulture);
            var format = "{0,-" + width + "}";

            System.Console.Write("- ");
            System.Console.Write(format, fieldLabel);
            System.Console.Write(" : ");

            return System.Console.ReadLine();
        }
    }
}