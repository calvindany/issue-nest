const userLocalStorage = {
  getItem(name) {
    const storage = localStorage.getItem("user");
    let user = {};
    if (storage) {
      user = JSON.parse(storage);
    }

    return user[name];
  },
  save(user) {
    return localStorage.setItem("user", JSON.stringify(user));
  },
};

export default userLocalStorage;
