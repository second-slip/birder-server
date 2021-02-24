export interface UserProfileViewModel {
    userName: string;
    avatar: string;
    registrationDate: Date | string;
    isOwnProfile: boolean;
    followersCount: number;
    followingCount: number;
    isFollowing: boolean;
    // followers: NetworkUserViewModel[];
    // following: NetworkUserViewModel[];
}

export interface NetworkUserViewModel {
    userName: string;
    avatar: string;
    isFollowing: boolean;
    isOwnProfile: boolean;
}

