import {
  Card,
  Stack,
  CardBody,
  Heading,
  Text,
  CardFooter,
  Button,
  Icon,
} from "@chakra-ui/react";
import React from "react";
import { AiFillFileText } from "react-icons/ai";
export default function FileCard() {
  return (
    <Card
      variant="filled"
      direction="row"
      align="center"
    //   textOverflow="ellipsis"
      w="90%"
    //   overflow="hidden"
    //   whiteSpace="nowrap"
    >
      <Icon as={AiFillFileText} boxSize="2em" ml="10%" />
      <Stack>
        <CardBody align="center" overflow="hidden" >
          <Text noOfLines="1">The Sacred Chairs PME, Official Readme</Text>
        </CardBody>
      </Stack>
    </Card>
  );
}
