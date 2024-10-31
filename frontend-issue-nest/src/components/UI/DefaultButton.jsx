import { Button } from "@mui/material";
export default function DefaultButton({
  variant,
  type,
  onclick,
  bgColor,
  children,
}) {
  const style = {};

  if (type == "primary") {
    style.backgroundColor = bgColor ? bgColor : "#091540";
  }
  return (
    <>
      <Button
        variant={variant}
        sx={{ padding: "5px 25px", ...style }}
        onClick={onclick}
      >
        {children}
      </Button>
    </>
  );
}
