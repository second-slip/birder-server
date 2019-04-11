import { UserViewModel } from './UserViewModel';

export interface UserProfileViewModel {
    userName: string;
    profileImage: string;
    registrationDate: Date | string;
    isOwnProfile: boolean;
    isFollowing: boolean;
    followers: NetworkUserViewModel[];
    following: NetworkUserViewModel[];
}

export interface NetworkUserViewModel {
    userName: string;
    profileImage: string;
    isFollowing: boolean;
}
