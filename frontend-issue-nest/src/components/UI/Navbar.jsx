export default function Navbar() {
  return (
    <>
      <nav className="flex justify-end py-5 px-4 bg-primary-i w-[100vw]">
        <div className="flex gap-4">
          <a href="">Tickets</a>
          <a href="">User Management</a>
          <a href="">Logout</a>
        </div>
      </nav>
    </>
  );
}
