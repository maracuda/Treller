﻿namespace SKBKontur.TaskManagerClient.BusinessObjects
{
    public enum ActionType
    {
        CreateCard,
        UpdateCard,
        CopyCard,
        DeleteCard,
        CommentCard,
        AddMemberToCard,
        RemoveMemberFromCard,
        UpdateCheckItemStateOnCard,
        AddAttachmentToCard,
        DeleteAttachmentFromCard,
        AddChecklistToCard,
        RemoveChecklistFromCard,
        MoveCardToBoard,
        MoveCardFromBoard,
        UpdateChecklist,
        ConvertToCardFromCheckItem,
        CopyCommentCard,

        CreateList = 100,
        UpdateList,

        CreateBoard,
        UpdateBoard,
        AddMemberToBoard,
        RemoveMemberFromBoard,
        AddToOrganizationBoard,
        RemoveFromOrganizationBoard,

        CreateOrganization,
        UpdateOrganization,
    }
}