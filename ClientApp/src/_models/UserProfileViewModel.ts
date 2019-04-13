import { UserViewModel } from './UserViewModel';

export interface UserProfileViewModel {
    userName: string;
    profileImage: string;
    registrationDate: Date | string;
    isOwnProfile: boolean;
    isFollowing: boolean;
    followers: FollowerViewModel[];
    following: NetworkUserViewModel[];
}

export interface NetworkUserViewModel {
    userName: string;
    profileImage: string;
    isFollowing: boolean;
    isOwnProfile: boolean;
}

export interface FollowerViewModel {
    userName: string;
    profileImage: string;
    isFollowing: boolean;
    isOwnProfile: boolean;
}
