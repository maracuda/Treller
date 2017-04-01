const avatarsArray = [
    "bear",
    "che",
    "enot",
    "giraffe",
    "glaza",
    "glaza2",
    "kotpirat",
    "lion",
    "lion2",
    "lis",
    "monkey",
    "mops",
    "panda",
    "svin",
    "tiger",
    "wolf"
];

export const getRandomAvatar = () => {
    const randomAvatarIndex = Math.floor(Math.random()*avatarsArray.length);

    return `/Content/img/avatar/${avatarsArray[randomAvatarIndex]}.jpg`;
};