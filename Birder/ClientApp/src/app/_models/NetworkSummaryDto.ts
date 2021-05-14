import { UserNetworkDto } from "./UserNetworkDto";

export interface NetworkSummaryDto {
    followersCount: number;
    followingCount: number;
    suggestedUsersToFollow: UserNetworkDto[];
}