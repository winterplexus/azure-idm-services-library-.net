//
//  ManageGroups.cs
//
//  Wiregrass Code Technology 2020-2023
//
using System.Globalization;

namespace IdentityManagement.Services.Console.Application
{
    internal sealed class ManageGroups
    {
        private readonly IIdentityManager identityManager;

        internal ManageGroups(IIdentityManager identityManager)
        {
            this.identityManager = identityManager;
        }

        internal async Task GetGroupByGroupName()
        {
            if (identityManager == null)
            {
                throw new IdentityManagerException("identityManager object is null");
            }
            if (identityManager.GroupServices == null)
            {
                throw new IdentityManagerException("identityManager.GroupServices object is null");
            }

            try
            {
                var groupName = ReadGroupName();
                if (groupName == null)
                {
                    WriteInvalidInput();
                    return;
                }

                var group = await identityManager.GroupServices.GetGroupByGroupName(groupName).ConfigureAwait(false);
                if (group == null)
                {
                    System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> group not found: {groupName}");
                    return;
                }
                else
                {
                    WriteGroup(group);
                }

                var groupMembers = await identityManager.GroupServices.GetGroupMembers(groupName).ConfigureAwait(false);
                if (groupMembers == null)
                {
                    System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> group members not found for group: {groupName}");
                    return;
                }
                else
                {
                    WriteGroupMembers(groupMembers);
                }

                var groupOwners = await identityManager.GroupServices.GetGroupOwners(groupName).ConfigureAwait(false);
                if (groupOwners == null)
                {
                    System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> group owners not found for group: {groupName}");
                    return;
                }
                else
                {
                    WriteGroupOwners(groupOwners);
                }
            }
            catch (IdentityManagerException ime)
            {
                WriteExceptionMessage(ime);
            }
            catch (ArgumentNullException ane)
            {
                WriteExceptionMessage(ane);
            }
            finally
            {
                ReadContinue();
            }
        }

        internal async Task GetGroupByObjectId()
        {
            if (identityManager == null)
            {
                throw new IdentityManagerException("identityManager object is null");
            }
            if (identityManager.GroupServices == null)
            {
                throw new IdentityManagerException("identityManager.GroupServices object is null");
            }

            try
            {
                var groupObjectId = ReadGroupObjectId();
                if (groupObjectId == null)
                {
                    WriteInvalidInput();
                    return;
                }

                var group = await identityManager.GroupServices.GetGroupByObjectId(groupObjectId).ConfigureAwait(false);
                if (group == null)
                {
                    System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> group not found: {groupObjectId} (group object ID)");
                    return;
                }
                else
                {
                    WriteGroup(group);
                }

                var groupMembers = await identityManager.GroupServices.GetGroupMembers(group.DisplayName).ConfigureAwait(false);
                if (groupMembers == null)
                {
                    System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> group members not found for group: {group.DisplayName}");
                    return;
                }
                else
                {
                    WriteGroupMembers(groupMembers);
                }

                var groupOwners = await identityManager.GroupServices.GetGroupOwners(group.DisplayName).ConfigureAwait(false);
                if (groupOwners == null)
                {
                    System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> group owners not found for group: {group.DisplayName}");
                    return;
                }
                else
                {
                    WriteGroupOwners(groupOwners);
                }
            }
            catch (IdentityManagerException ime)
            {
                WriteExceptionMessage(ime);
            }
            catch (ArgumentNullException ane)
            {
                WriteExceptionMessage(ane);
            }
            finally
            {
                ReadContinue();
            }
        }

        internal async Task GetGroups()
        {
            if (identityManager == null)
            {
                throw new IdentityManagerException("identityManager object is null");
            }
            if (identityManager.GroupServices == null)
            {
                throw new IdentityManagerException("identityManager.GroupServices object is null");
            }

            var limit = ReadLimit();

            try
            {
                var groupsList = await identityManager.GroupServices.GetGroups(limit).ConfigureAwait(false);
                if (groupsList == null)
                {
                    return;
                }

                var counter = 0;
                foreach (var group in groupsList)
                {
                    WriteGroup(group);

                    var groupMembers = await identityManager.GroupServices.GetGroupMembers(group.DisplayName).ConfigureAwait(false);
                    if (groupMembers == null)
                    {
                        System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> group members not found for group: {group.DisplayName}");
                        return;
                    }
                    else
                    {
                        WriteGroupMembers(groupMembers);
                    }

                    var groupOwners = await identityManager.GroupServices.GetGroupOwners(group.DisplayName).ConfigureAwait(false);
                    if (groupOwners == null)
                    {
                        System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> group owners not found for group: {group.DisplayName}");
                        return;
                    }
                    else
                    {
                        WriteGroupOwners(groupOwners);
                    }

                    counter++;
                }

                WriteRecordCount(counter);
            }
            catch (IdentityManagerException ime)
            {
                WriteExceptionMessage(ime);
            }
            catch (ArgumentNullException ane)
            {
                WriteExceptionMessage(ane);
            }
            finally
            {
                ReadContinue();
            }
        }

        internal async Task GetGroupsByName()
        {
            if (identityManager == null)
            {
                throw new IdentityManagerException("identityManager object is null");
            }
            if (identityManager.GroupServices == null)
            {
                throw new IdentityManagerException("identityManager.GroupServices object is null");
            }

            var groupName = ReadGroupName();
            if (groupName == null)
            {
                WriteInvalidInput();
                return;
            }

            try
            {
                var groupsList = await identityManager.GroupServices.GetGroupsByName(groupName).ConfigureAwait(false);
                if (groupsList == null)
                {
                    return;
                }

                var counter = 0;
                foreach (var group in groupsList)
                {
                    WriteGroup(group);

                    var groupMembers = await identityManager.GroupServices.GetGroupMembers(group.DisplayName).ConfigureAwait(false);
                    if (groupMembers == null)
                    {
                        System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> group members not found for group: {group.DisplayName}");
                        return;
                    }
                    else
                    {
                        WriteGroupMembers(groupMembers);
                    }

                    var groupOwners = await identityManager.GroupServices.GetGroupOwners(group.DisplayName).ConfigureAwait(false);
                    if (groupOwners == null)
                    {
                        System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> group owners not found for group: {group.DisplayName}");
                        return;
                    }
                    else
                    {
                        WriteGroupOwners(groupOwners);
                    }

                    counter++;
                }

                WriteRecordCount(counter);
            }
            catch (IdentityManagerException ime)
            {
                WriteExceptionMessage(ime);
            }
            catch (ArgumentNullException ane)
            {
                WriteExceptionMessage(ane);
            }
            finally
            {
                ReadContinue();
            }
        }

        internal async Task CreateGroup()
        {
            if (identityManager == null)
            {
                throw new IdentityManagerException("identityManager object is null");
            }
            if (identityManager.GroupServices == null)
            {
                throw new IdentityManagerException("identityManager.GroupServices object is null");
            }

            try
            {
                var groupModel = ReadGroup();

                var group = await identityManager.GroupServices.GetGroupByGroupName(groupModel.DisplayName).ConfigureAwait(false);
                if (group == null)
                {
                    var groupObjectId = await identityManager.GroupServices.CreateGroup(groupModel).ConfigureAwait(false);

                    WriteGroupObjectIdentifier(groupObjectId);
                }
                else
                {
                    System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> display name already exists (choose another group display name)");
                }
            }
            catch (IdentityManagerException ime)
            {
                WriteExceptionMessage(ime);
            }
            catch (ArgumentNullException ane)
            {
                WriteExceptionMessage(ane);
            }
            finally
            {
                ReadContinue();
            }
        }

        internal async Task DeleteGroup()
        {
            if (identityManager == null)
            {
                throw new IdentityManagerException("identityManager object is null");
            }
            if (identityManager.GroupServices == null)
            {
                throw new IdentityManagerException("identityManager.GroupServices object is null");
            }

            try
            {
                var groupName = ReadGroupName();
                if (groupName == null)
                {
                    WriteInvalidInput();
                    return;
                }

                var status = await identityManager.GroupServices.DeleteGroup(groupName).ConfigureAwait(false);
            }
            catch (IdentityManagerException ime)
            {
                WriteExceptionMessage(ime);
            }
            catch (ArgumentNullException ane)
            {
                WriteExceptionMessage(ane);
            }
            finally
            {
                ReadContinue();
            }
        }

        internal async Task AddMemberToGroup()
        {
            if (identityManager == null)
            {
                throw new IdentityManagerException("identityManager object is null");
            }
            if (identityManager.GroupServices == null)
            {
                throw new IdentityManagerException("identityManager.GroupServices object is null");
            }

            try
            {
                var groupName = ReadGroupName();
                if (groupName == null)
                {
                    WriteInvalidInput();
                    return;
                }

                var userName = ReadUserName();
                if (userName == null)
                {
                    WriteInvalidInput();
                    return;
                }

                var status = await identityManager.GroupServices.AddMemberToGroup(groupName, userName).ConfigureAwait(false);
            }
            catch (IdentityManagerException ime)
            {
                WriteExceptionMessage(ime);
            }
            catch (ArgumentNullException ane)
            {
                WriteExceptionMessage(ane);
            }
            finally
            {
                ReadContinue();
            }
        }

        internal async Task RemoveMemberToGroup()
        {
            if (identityManager == null)
            {
                throw new IdentityManagerException("identityManager object is null");
            }
            if (identityManager.GroupServices == null)
            {
                throw new IdentityManagerException("identityManager.GroupServices object is null");
            }

            try
            {
                var groupName = ReadGroupName();
                if (groupName == null)
                {
                    WriteInvalidInput();
                    return;
                }

                var userName = ReadUserName();
                if (userName == null)
                {
                    WriteInvalidInput();
                    return;
                }

                var status = await identityManager.GroupServices.RemoveMemberFromGroup(groupName, userName).ConfigureAwait(false);
            }
            catch (IdentityManagerException ime)
            {
                WriteExceptionMessage(ime);
            }
            catch (ArgumentNullException ane)
            {
                WriteExceptionMessage(ane);
            }
            finally
            {
                ReadContinue();
            }
        }

        internal async Task AddOwnerToGroup()
        {
            if (identityManager == null)
            {
                throw new IdentityManagerException("identityManager object is null");
            }
            if (identityManager.GroupServices == null)
            {
                throw new IdentityManagerException("identityManager.GroupServices object is null");
            }

            try
            {
                var groupName = ReadGroupName();
                if (groupName == null)
                {
                    WriteInvalidInput();
                    return;
                }

                var userName = ReadUserName();
                if (userName == null)
                {
                    WriteInvalidInput();
                    return;
                }

                var status = await identityManager.GroupServices.AddOwnerToGroup(groupName, userName).ConfigureAwait(false);
            }
            catch (IdentityManagerException ime)
            {
                WriteExceptionMessage(ime);
            }
            catch (ArgumentNullException ane)
            {
                WriteExceptionMessage(ane);
            }
            finally
            {
                ReadContinue();
            }
        }

        internal async Task RemoveOwnerToGroup()
        {
            if (identityManager == null)
            {
                throw new IdentityManagerException("identityManager object is null");
            }
            if (identityManager.GroupServices == null)
            {
                throw new IdentityManagerException("identityManager.GroupServices object is null");
            }

            try
            {
                var groupName = ReadGroupName();
                if (groupName == null)
                {
                    WriteInvalidInput();
                    return;
                }

                var userName = ReadUserName();
                if (userName == null)
                {
                    WriteInvalidInput();
                    return;
                }

                var status = await identityManager.GroupServices.RemoveOwnerFromGroup(groupName, userName).ConfigureAwait(false);
            }
            catch (IdentityManagerException ime)
            {
                WriteExceptionMessage(ime);
            }
            catch (ArgumentNullException ane)
            {
                WriteExceptionMessage(ane);
            }
            finally
            {
                ReadContinue();
            }
        }

        private static void ReadContinue()
        {
            System.Console.WriteLine("");
            System.Console.Write("PRESS ENTER TO CONTINUE");
            System.Console.ReadKey();
        }        

        private static int ReadLimit()
        {
            System.Console.WriteLine("");
            System.Console.Write("ENTER TOP GROUPS LIMIT: ");

            var line = System.Console.ReadLine();
            return !int.TryParse(line, out var number) ? 0 : number;
        }

        private static string? ReadUserName()
        {
            System.Console.WriteLine("");

            return GetInputField("USER NAME", 20);
        }

        private static string? ReadGroupName()
        {
            System.Console.WriteLine("");

            return GetInputField("GROUP NAME", 20);
        }

        private static string? ReadGroupObjectId()
        {
            System.Console.WriteLine("");

            return GetInputField("GROUP OBJECT ID", 20);
        }

        private static GroupModel ReadGroup()
        {
            var groupModel = new GroupModel
            {
                DisplayName  = GetInputField("DISPLAY NAME", 20),
                Description  = GetInputField("DESCRIPTION", 20),
                MailNickname = GetInputField("MAIL NICK NAME", 20)
            };

            return groupModel;
        }

        private static void WriteGroup(GroupModel groupModel)
        {
            if (groupModel == null)
            {
                System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> no group found");
                return;
            }

            System.Console.WriteLine("");
            System.Console.WriteLine("GROUP");
            System.Console.WriteLine("--------------------------------------------------------------------------------");
            System.Console.WriteLine($"OBJECT ID            = {groupModel.Id}");
            System.Console.WriteLine($"SECURITY ENABLED     = {groupModel.SecurityEnabled}");
            System.Console.WriteLine($"CREATED DATE TIME    = {groupModel.CreatedDateTime}");
            System.Console.WriteLine($"DISPLAY NAME         = {groupModel.DisplayName}");
            System.Console.WriteLine($"DESCRIPTION          = {groupModel.Description}");
        }

        private static void WriteGroupObjectIdentifier(string groupObjectId)
        {
            System.Console.WriteLine("");
            System.Console.WriteLine($"GROUP OBJECT ID      = {groupObjectId}");
        }

        private static void WriteGroupMembers(IList<UserModel?> groupMembers)
        {
            if (groupMembers == null)
            {
                return;
            }

            System.Console.WriteLine($"GROUP MEMBERS        = {groupMembers.Count} (COUNT)");
            foreach (var member in groupMembers)
            {
                if (member == null)
                {
                    System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> group member is empty (null)");
                }
                else
                {
                    System.Console.WriteLine($"MEMBER ............... USER OBJECT ID     = {member.Id}");
                    System.Console.WriteLine($"                     = USER DISPLAY NAME  = {member.DisplayName}");
                }
            }
        }

        private static void WriteGroupOwners(IList<UserModel?> groupOwners)
        {
            if (groupOwners == null)
            {
                return;
            }

            System.Console.WriteLine($"GROUP OWNERS         = {groupOwners.Count} (COUNT)");
            foreach (var member in groupOwners)
            {
                if (member == null)
                {
                    System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> group owner is empty (null)");
                }
                else
                {

                    System.Console.WriteLine($"OWNER ................ USER OBJECT ID     = {member.Id}");
                    System.Console.WriteLine($"                     = USER DISPLAY NAME  = {member.DisplayName}");
                }
            }
        }

        private static void WriteRecordCount(int counter)
        {
            System.Console.WriteLine("");
            System.Console.WriteLine($"TOTAL RECORDS: {counter}");
        }

        private static void WriteInvalidInput()
        {
            System.Console.WriteLine($"{Environment.NewLine}ERROR-> invalid input");
        }

        private static void WriteExceptionMessage(Exception ex)
        {
            System.Console.WriteLine($"EXCEPTION-> {ex.Message}");
            System.Console.WriteLine($"INNER EXCEPTION-> {ex.InnerException?.Message}");
            System.Console.WriteLine($"STACK TRACE-> {Environment.NewLine}{ex.StackTrace}");
        }

        private static string? GetInputField(string fieldLabel, int fieldLabelWidth)
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

            var field = System.Console.ReadLine();
            if (field != null && field.Length < 1)
            {
                field = null;
            }
            return field;
        }
    }
}