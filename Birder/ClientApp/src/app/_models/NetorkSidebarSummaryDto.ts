import { UserNetworkDto } from "./UserNetworkDto";

export interface NetworkSidebarSummaryDto {
    followersCount: number;
    followingCount: number;
    suggestedUsersToFollow: UserNetworkDto[];
}