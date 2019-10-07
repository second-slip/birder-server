import { NetworkUserViewModel } from './UserProfileViewModel';

export interface UserNetworkDto {
    followers: NetworkUserViewModel[];
    following: NetworkUserViewModel[];
}
