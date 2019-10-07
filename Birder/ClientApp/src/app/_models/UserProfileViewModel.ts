export interface UserProfileViewModel {
    userName: string;
    avatar: string;
    registrationDate: Date | string;
    isOwnProfile: boolean;
    isFollowing: boolean;
    followers: NetworkUserViewModel[];
    following: NetworkUserViewModel[];
}

export interface NetworkUserViewModel {
    userName: string;
    avatar: string;
    isFollowing: boolean;
    isOwnProfile: boolean;
}

