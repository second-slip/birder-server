export interface UserProfileViewModel {
    userName: string;
    profileImage: string;
    registrationDate: Date | string;
    isOwnProfile: boolean;
    isFollowing: boolean;
    followersCount: number;
    followingCount: number;
}
